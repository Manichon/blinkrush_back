# blinkrush_back

API ASP.NET Core pour enregistrer et consulter les scores BlinkRush (Speed Run et Endurance), stockés dans SQLite.

## Prérequis

- SDK .NET 10 (le projet cible `net10.0`).

## Lancer en local

```bash
cd BlinkRushBack
dotnet run --launch-profile http
```

- API : `http://localhost:5281`
- Fichier SQLite : `BlinkRushBack/Data/blinkrush.db` (créé au premier démarrage ; les migrations EF s’appliquent automatiquement).

### Endpoints

| Méthode | Chemin | Description |
|--------|--------|-------------|
| `POST` | `/api/records` | Corps JSON : `mode` (`speedRun` \| `endurance`), `value` (nombre), `occurredAt` (optionnel, ISO 8601). |
| `GET` | `/api/leaderboard?mode=speedRun` ou `mode=endurance` (`take` optionnel, défaut 20) | Classement : Speed Run par temps croissant, Endurance par nombre de clignements décroissant. |

Exemples : voir [`BlinkRushBack/BlinkRush.http`](BlinkRushBack/BlinkRush.http).

## Docker

Depuis le dossier `BlinkRushBack` (là où se trouve le `Dockerfile`) :

```bash
docker compose -f compose.yaml up --build
```

L’API écoute sur le port **8080**. Les données sont persistées dans le volume `blinkrush-data` monté sur `/app/Data`.

### Déploiement VPS (GHCR)

Option recommandée : pull l'image publiée par la CI GitHub Actions.

```bash
docker login ghcr.io -u Manichon
docker pull ghcr.io/manichon/blinkrush-back:latest
cd BlinkRushBack
docker compose -f compose.prod.yaml up -d
```

Si le package GHCR est privé, utilise un token GitHub (PAT) avec permission `read:packages`.

## Migrations

Les migrations sont dans `BlinkRushBack/Data/Migrations/`. Pour en ajouter une après modification du modèle :

```bash
cd BlinkRushBack
dotnet ef migrations add NomDeLaMigration
dotnet ef database update
```
