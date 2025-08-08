import 'dotenv/config';
import fetch from 'node-fetch';
import {
  Client,
  GatewayIntentBits,
  REST,
  Routes,
  SlashCommandBuilder,
  TextChannel
} from 'discord.js';
import { buildCouncilReportEmbed, scheduleNightlyCouncilReport, dispatchReport } from './nightlyReport.js';
import { broadcastHandshake } from './handshake.js';

const TOKEN = process.env.DISCORD_TOKEN!;
const CLIENT_ID = process.env.CLIENT_ID!;
const GUILD_ID = process.env.GUILD_ID!;
const OWNER_ID = process.env.OWNER_ID!;
const MCP = process.env.MCP_URL;
const COUNCIL_CH = process.env.CHN_COUNCIL!;
const SIBLING_HANDSHAKES = (process.env.SIBLING_HANDSHAKES || '')
  .split(',')
  .map((s) => s.trim())
  .filter(Boolean);

const commands = [
  new SlashCommandBuilder()
    .setName('council')
    .setDescription('ShadowFlower council utilities')
    .addSubcommandGroup((g) =>
      g
        .setName('report')
        .setDescription('Council reporting')
        .addSubcommand((sc) => sc.setName('now').setDescription('Send council report now'))
    )
].map((c) => c.toJSON());

const rest = new REST({ version: '10' }).setToken(TOKEN);
(async () => {
  try {
    await rest.put(Routes.applicationGuildCommands(CLIENT_ID, GUILD_ID), { body: commands });
  } catch (e) {
    console.error('Failed to register commands', e);
  }
})();

const client = new Client({
  intents: [
    GatewayIntentBits.Guilds,
    GatewayIntentBits.GuildMessages,
    GatewayIntentBits.MessageContent
  ]
});

client.once('ready', () => {
  console.log(`Logged in as ${client.user?.tag}`);
  scheduleNightlyCouncilReport(client);
  if (SIBLING_HANDSHAKES.length) {
    // notify sibling services of our presence
    broadcastHandshake(SIBLING_HANDSHAKES);
  }
});

client.on('interactionCreate', async (interaction) => {
  if (!interaction.isChatInputCommand()) return;
  if (
    interaction.commandName === 'council' &&
    interaction.options.getSubcommandGroup() === 'report' &&
    interaction.options.getSubcommand() === 'now'
  ) {
    if (interaction.user.id !== OWNER_ID) {
      await interaction.reply({
        content: 'Only the council owner may invoke this.',
        ephemeral: true
      });
      return;
    }
    const emb = await buildCouncilReportEmbed();
    const ch = client.channels.cache.get(COUNCIL_CH) as TextChannel | undefined;
    await dispatchReport(client, emb);
    await interaction.reply({ content: 'Council report dispatched.', ephemeral: true });
    // Mirror report into channel if dispatch used webhook
    if (!ch) return;
  }
});

client.on('messageCreate', async (msg) => {
  if (msg.author.bot || !MCP) return;
  const match = msg.content.match(/^!guardian\s+(\w+)\s+(.+)/i);
  if (match) {
    const [, guardian, message] = match;
    try {
      await fetch(`${MCP}/osc`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ address: guardian, value: message })
      });
      await msg.reply(`Routed to ${guardian}.`);
    } catch (err) {
      console.error('Relay error', err);
      await msg.reply('Failed to relay message.');
    }
  }
});

client.login(TOKEN);
