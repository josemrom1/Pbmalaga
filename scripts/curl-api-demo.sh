#!/usr/bin/env bash

set -euo pipefail

BASE_URL="${BASE_URL:-https://jsonplaceholder.typicode.com}"
API_KEY="${API_KEY:-}"

usage() {
  cat <<'EOF'
Uso:
  BASE_URL='https://api.example.com' API_KEY='tu_token' ./scripts/curl-api-demo.sh GET /todos/1
  BASE_URL='https://api.example.com' API_KEY='tu_token' ./scripts/curl-api-demo.sh POST /posts '{"title":"Hola","body":"Demo","userId":1}'

Argumentos:
  METHOD   GET | POST | PUT | PATCH | DELETE
  PATH     Ruta relativa, por ejemplo /todos/1
  DATA     JSON opcional para POST/PUT/PATCH

Variables de entorno:
  BASE_URL  URL base de la API
  API_KEY   Token o clave de acceso (si la necesita tu API)
EOF
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" || $# -lt 2 ]]; then
  usage
  exit 0
fi

METHOD="${1^^}"
PATH="${2}"
DATA="${3:-}"

if [[ "$METHOD" != "GET" && "$METHOD" != "POST" && "$METHOD" != "PUT" && "$METHOD" != "PATCH" && "$METHOD" != "DELETE" ]]; then
  echo "Método no soportado: $METHOD" >&2
  usage
  exit 1
fi

if [[ -z "$PATH" ]]; then
  echo "La ruta PATH es obligatoria." >&2
  usage
  exit 1
fi

CURL_ARGS=("-sS" "-H" "Accept: application/json" "-H" "Content-Type: application/json")

if [[ -n "$API_KEY" ]]; then
  CURL_ARGS+=("-H" "Authorization: Bearer $API_KEY")
fi

# Nota: para WalletWallet puedes ejecutar:
#   BASE_URL='https://api.walletwallet.dev' API_KEY='ww_live_XXXX' ./scripts/curl-api-demo.sh GET /api/auth/usage

URL="${BASE_URL%/}${PATH}"

if [[ "$METHOD" == "GET" || "$METHOD" == "DELETE" ]]; then
  curl "${CURL_ARGS[@]}" -X "$METHOD" "$URL"
else
  if [[ -z "$DATA" ]]; then
    echo "Para POST/PUT/PATCH debes enviar el cuerpo JSON como tercer argumento." >&2
    exit 1
  fi
  curl "${CURL_ARGS[@]}" -X "$METHOD" "$URL" --data "$DATA"
fi
