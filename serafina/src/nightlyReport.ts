import 'dotenv/config';
import fetch from 'node-fetch';
import { EmbedBuilder, TextChannel, Client } from 'discord.js';
import cron from 'node-cron';

const MCP = process.env.MCP_URL!;
const COUNCIL_CH = process.env.CHN_COUNCIL!;
const LILY_WEBHOOK = process.env.WH_LILYBEAR; // optional pretty sender
const GH_REPOS = (process.env.NAV_REPOS || '')
  .split(',')
  .map((s) => s.trim())
  .filter(Boolean);

/**
 * Build the embed used for council reports.
 */
export async function buildCouncilReportEmbed(): Promise<EmbedBuilder> {
  const mcp = await getMcpStatus();
  const repoLines = GH_REPOS.length
    ? (await Promise.all(GH_REPOS.map(getRepoDigest))).join('\n')
    : '—';

  return new EmbedBuilder()
    .setTitle('\uD83C\uDF19 Nightly Council Report')
    .setDescription('Summary of the last 24h across our realm.')
    .setColor(0x9b59b6)
    .addFields(
      { name: 'System Health (MCP)', value: mcp.slice(0, 1024) || '—' },
      { name: 'Recent Commits', value: repoLines.slice(0, 1024) || '—' }
    )
    .setFooter({ text: 'Reported by Lilybear' })
    .setTimestamp(new Date());
}

async function getMcpStatus() {
  try {
    const r = await fetch(`${MCP}/ask-gemini`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ prompt: 'Summarize system health in one sentence.' })
    });
    const j = await r.json().catch(() => ({ response: '(no data)' }));
    return j.response || '(no data)';
  } catch {
    return '(MCP unreachable)';
  }
}

async function getRepoDigest(repo: string) {
  const since = new Date(Date.now() - 24 * 60 * 60 * 1000).toISOString();
  const url = `https://api.github.com/repos/${repo}/commits?since=${encodeURIComponent(since)}&per_page=5`;
  try {
    const r = await fetch(url, { headers: { Accept: 'application/vnd.github+json' } });
    if (!r.ok) return `• ${repo}: no recent commits`;
    const commits = (await r.json()) as any[];
    if (!commits.length) return `• ${repo}: 0 commits in last 24h`;
    const lines = commits.map(
      (c) => `• ${repo}@${(c.sha || '').slice(0, 7)} — ${c.commit.message.split('\n')[0]}`
    );
    return lines.join('\n');
  } catch {
    return `• ${repo}: (error fetching commits)`;
  }
}

/**
 * Schedule nightly report at 08:00 UTC.
 */
export function scheduleNightlyCouncilReport(client: Client) {
  cron.schedule('0 8 * * *', async () => {
    const emb = await buildCouncilReportEmbed();
    await dispatchReport(client, emb);
  });
}

/**
 * Dispatch a council report embed to the configured destination.
 */
export async function dispatchReport(client: Client, emb: EmbedBuilder) {
  try {
    if (LILY_WEBHOOK) {
      await fetch(LILY_WEBHOOK, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ embeds: [emb.toJSON()] })
      });
    } else {
      const ch = client.channels.cache.get(COUNCIL_CH) as TextChannel | undefined;
      await ch?.send({ embeds: [emb] });
    }
  } catch (e) {
    console.error('Nightly report dispatch error:', e);
  }
}
