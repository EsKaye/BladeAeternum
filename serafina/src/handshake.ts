import fetch from 'node-fetch';
import pkg from '../package.json' assert { type: 'json' };

/**
 * Standardized handshake payload shared across ShadowFlower repos.
 * Each service announces itself so peers can discover capabilities.
 */
export interface HandshakePayload {
  repo: string; // repository or service name
  version: string; // semantic version
  capabilities: string[]; // high-level feature flags
  timestamp: string; // ISO timestamp of handshake emission
}

/**
 * Broadcast a handshake to the provided endpoints.
 * @param endpoints List of HTTP URLs expecting the handshake payload.
 */
export async function broadcastHandshake(endpoints: string[]): Promise<void> {
  const payload: HandshakePayload = {
    repo: pkg.name || 'unknown',
    version: pkg.version || '0.0.0',
    capabilities: ['router', 'reports'], // keep lightweight; expand as features grow
    timestamp: new Date().toISOString()
  };

  for (const url of endpoints) {
    try {
      await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
      console.log(`[handshake] sent to ${url}`);
    } catch (err) {
      // don't crash if a peer is down; just log for ops visibility
      console.error(`[handshake] failed for ${url}`, err);
    }
  }
}
