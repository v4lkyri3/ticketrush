#!/usr/bin/env bash
# Verifies the local toolchain for TicketRush. Run from the repo root:  bash scripts/check-prereqs.sh
# On Windows use Git Bash or WSL2.

ok=0
check() {
  local name="$1" cmd="$2" hint="$3"
  if command -v "$cmd" >/dev/null 2>&1; then
    printf "  [ok]   %-14s %s\n" "$name" "$("$cmd" --version 2>&1 | head -n1)"
  else
    printf "  [MISS] %-14s %s\n" "$name" "$hint"
    ok=1
  fi
}

echo "TicketRush prerequisites"
check ".NET SDK" dotnet "install .NET 10 SDK (LTS): https://dotnet.microsoft.com/download"
check "Node.js"  node   "install Node.js LTS: https://nodejs.org"
check "npm"      npm    "ships with Node.js"
check "Docker"   docker "install Docker Desktop, Rancher Desktop, or Docker Engine"
check "Git"      git    "https://git-scm.com"

if command -v dotnet >/dev/null 2>&1; then
  major="$(dotnet --version | cut -d. -f1)"
  if [ "$major" -lt 10 ]; then
    echo "  [WARN] .NET SDK is $(dotnet --version); this project targets .NET 10 (LTS)."
    ok=1
  fi
fi

if command -v docker >/dev/null 2>&1 && ! docker info >/dev/null 2>&1; then
  echo "  [WARN] Docker is installed but the daemon is not running."
  ok=1
fi

[ "$ok" -eq 0 ] && echo "All good." || echo "Fix the items above, then re-run."
exit "$ok"
