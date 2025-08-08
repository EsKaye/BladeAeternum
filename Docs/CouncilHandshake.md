# ShadowFlower Council Handshake Protocol

Each service within the MKWW/GameDin network announces its presence to peers via a simple HTTP POST.
This enables light‑weight service discovery without central coordination.

## Payload

```json
{
  "repo": "serafina-bot",
  "version": "0.1.0",
  "capabilities": ["router", "reports"],
  "timestamp": "2025-05-01T00:00:00.000Z"
}
```

- **repo**: repository or service identifier.
- **version**: semantic version string.
- **capabilities**: feature flags or roles exposed by the service.
- **timestamp**: ISO-8601 emission time.

## Endpoints

Each bot exposes a `/handshake` endpoint accepting the payload above.
Upon startup, bots broadcast to the URLs listed in `SIBLING_HANDSHAKES`.

Services should respond with HTTP 204 on success.

## Example

```
SIBLING_HANDSHAKES="https://lilybear.ops/handshake,https://athena.ops/handshake"
```

On boot Serafina will POST the payload to each URL and log success or failure.
This mesh keeps the Council aware of which guardians are awake.

