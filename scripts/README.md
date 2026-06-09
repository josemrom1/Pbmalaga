# cURL API helper

Este directorio añade un flujo simple para hacer llamadas a una API con cURL desde la terminal.

## Uso rápido

1. Para la llamada que quieres lanzar a WalletWallet:

```bash
curl https://api.walletwallet.dev/api/auth/usage \
  -H "Authorization: Bearer ww_live_XXXX"
```

2. Si quieres usar el helper del proyecto, puedes hacerlo así:

```bash
BASE_URL='https://api.walletwallet.dev' \
API_KEY='ww_live_XXXX' \
./scripts/curl-api-demo.sh GET /api/auth/usage
```

3. Para crear o enviar datos a otra API:

```bash
BASE_URL='https://api.example.com' \
API_KEY='tu_token' \
./scripts/curl-api-demo.sh POST /posts '{"title":"Hola","body":"Demo","userId":1}'
```

3. Si la API no requiere autenticación, puedes omitir API_KEY.

## Notas
- El script usa cURL de forma simple con headers JSON.
- Para WalletWallet, el header que necesitas es exactamente: Authorization: Bearer ww_live_XXXX
- Si tu API requiere otro nombre de header, puedes adaptarlo en el script.
