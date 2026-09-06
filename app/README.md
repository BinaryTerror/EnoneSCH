# School API

ASP.NET Core 10 REST API for **Enoneno's School** — plataforma de explicações em vídeo.

## Arquitetura

```
School.Domain          - Entidades, interfaces
School.Application     - Casos de uso, DTOs
School.Infrastructure  - EF Core, repositórios, storage (local / S3-R2)
School.Api             - Controllers, auth JWT, configuração
```

## Stack de Deployment

| Camada         | Serviço            |
|----------------|--------------------|
| Frontend       | Vercel (Next.js)   |
| Backend API    | Render (Docker)    |
| PostgreSQL     | Neon               |
| Vídeos/objetos | Cloudflare R2      |
| Código/CI      | GitHub Actions     |

---

## Variáveis de Ambiente

### Backend (`app/.env`)

```bash
DATABASE_CONNECTION_STRING=Host=...;Port=5432;Database=school;Username=...;Password=...;SslMode=Require;Trust Server Certificate=true
JWT_SECRET=...                  # mínimo 32 caracteres
JWT_ISSUER=SchoolApi
JWT_AUDIENCE=SchoolApp
CORS_ORIGINS=https://meu-site.vercel.app,http://localhost:3000,http://localhost:3001
STORAGE_ENDPOINT=https://<account-id>.r2.cloudflarestorage.com
STORAGE_ACCESS_KEY=...
STORAGE_SECRET_KEY=...
STORAGE_BUCKET=meu-bucket
```

> Se `STORAGE_*` estiverem vazias, o backend usa storage **local** (`uploads/`) — útil em dev e em testes. Em produção use Cloudflare R2.

### Frontend (`web/.env.local`)

```bash
NEXT_PUBLIC_API_URL=https://school-api.onrender.com
```

---

## Executar localmente

### Backend

```bash
cd app/src/School.Api
dotnet restore
dotnet ef database update
dotnet run --urls http://localhost:5282
```

### Frontend

```bash
cd web
npm install
npm run dev       # http://localhost:3001
```

### Docker (dev com PostgreSQL incluído)

```bash
cd app
cp .env.example .env   # editar secrets
docker compose up -d --build
```

---

## Migrations

Criar nova migration:

```bash
cd app/src/School.Infrastructure
dotnet ef migrations add NomeDaMigracao --startup-project ../School.Api
```

Aplicar:

```bash
cd app/src/School.Api
dotnet ef database update
```

> Em produção (Render + Neon), rode as migrations a partir do container apontando para a `DATABASE_CONNECTION_STRING` do Neon.

---

## Deploy

### 1. GitHub

```bash
git init
git add .
git commit -m "feat: Enoneno's School"
git branch -M main
git remote add origin https://github.com/<user>/school.git
git push -u origin main
```

### 2. Backend → Render

1. **New → Blueprint** e aponta para o repositório. O `render.yaml` cria a web service a partir do `Dockerfile`.
2. Defina no painel (durante a primeira criação) as variáveis com `sync: false`:
   - `JWT_SECRET`, `JWT_ISSUER`, `JWT_AUDIENCE`
   - `CORS_ORIGINS`
   - `STORAGE_ENDPOINT`, `STORAGE_ACCESS_KEY`, `STORAGE_SECRET_KEY`, `STORAGE_BUCKET`
3. O health check (`/health`) é usado para marcar o serviço como "Live".

### 3. Banco → Neon

1. Crie um projeto PostgreSQL na Neon.
2. Copie a connection string e cole em `DATABASE_CONNECTION_STRING` no Render.
3. (Opcional) Aplique as migrations ou rode o `seed_all.sql` para dados de demonstração.

### 4. Frontend → Vercel

1. Importe o repositório no Vercel → framework **Next.js**.
2. Em **Environment Variables** defina `NEXT_PUBLIC_API_URL` para a URL do backend no Render.
3. Deploy. O `vercel.json` já está configurado.

### 5. Vídeos → Cloudflare R2

1. Crie um bucket no R2 (ex.: `school-videos`).
2. Crie um **API Token** com permissão de leitura/escrita no bucket.
3. Preencha `STORAGE_ENDPOINT`, `STORAGE_ACCESS_KEY`, `STORAGE_SECRET_KEY`, `STORAGE_BUCKET` no Render.
4. (Opcional) Ative o domínio custom do bucket e o CDN da Cloudflare para servir os vídeos.

---

## Endpoints principais

| Método | Rota | Role |
|--------|------|------|
| POST | `/api/auth/register` | Público |
| POST | `/api/auth/login` | Público |
| POST | `/api/auth/refresh` | Público |
| POST | `/api/auth/logout` | Autenticado |
| GET | `/api/auth/me` | Autenticado |
| GET | `/api/courses` | Autenticado |
| GET | `/api/courses/{id}/years` | Autenticado |
| GET | `/api/academic-years/{id}/semesters` | Autenticado |
| GET | `/api/semesters/{id}/subjects` | Autenticado |
| GET | `/api/subjects/{id}/lessons` | Autenticado |
| GET | `/api/lessons/{id}/video` | Autenticado |
| GET | `/api/videos/{id}/stream` | Autenticado |
| GET | `/api/progress/me` | Autenticado |
| POST | `/api/progress` | Autenticado |
| POST | `/api/admin/courses` | Admin |
| PUT | `/api/admin/courses/{id}` | Admin |
| DELETE | `/api/admin/courses/{id}` | Admin |
| POST | `/api/admin/academic-years/{courseId}` | Admin |
| POST | `/api/admin/lessons/{subjectId}` | Admin/Teacher |
| POST | `/api/admin/videos/{lessonId}` | Admin/Teacher |
| GET | `/health` | Público |

## Utilizadores de dev (seed automático)

| Email | Password | Role |
|-------|----------|------|
| admin@school.com | admin123 | Admin |
| student@school.com | student123 | Student |

> ⚠️ Apenas para desenvolvimento. Nunca usar em produção.

---

## License
MIT