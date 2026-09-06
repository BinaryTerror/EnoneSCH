# Lumina Learn

corrija alguns erros, coloque sistema de seguranca contra os videos, permita fazer login com o google, coloque sistema de seguranca: <!DOCTYPE html>
<html lang="pt">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=yes">
    <title>Enoneno's School • Plataforma de Explicações</title>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        :root {
            --bg: #f0f4ff; --surface: #ffffff; --primary: #4f46e5; --primary-light: #7c3aed;
            --accent: #f97316; --text: #1e293b; --text-secondary: #64748b;
            --border: rgba(0,0,0,0.06); --shadow: 0 10px 30px rgba(0,0,0,0.05);
            --radius: 18px; --transition: 0.25s cubic-bezier(0.2, 0.9, 0.4, 1);
        }
        body.dark {
            --bg: #0f172a; --surface: #1e293b; --primary: #818cf8; --primary-light: #a5b4fc;
            --accent: #fb923c; --text: #f1f5f9; --text-secondary: #94a3b8;
            --border: rgba(255,255,255,0.06); --shadow: 0 10px 30px rgba(0,0,0,0.4);
        }
        body {
            font-family: 'Poppins', sans-serif; background: var(--bg); color: var(--text);
            min-height: 100vh; display: flex; align-items: center; justify-content: center;
            transition: background 0.3s, color 0.3s; padding: 1.5rem;
        }
        body::before {
            content: ""; position: fixed; top: -50%; left: -50%; width: 200%; height: 200%;
            background: radial-gradient(circle at 30% 40%, rgba(79,70,229,0.06), transparent 50%),
                        radial-gradient(circle at 70% 60%, rgba(249,115,22,0.04), transparent 50%);
            z-index: 0; pointer-events: none;
        }
        .btn {
            background: linear-gradient(135deg, var(--primary), var(--primary-light)); color: white;
            border: none; border-radius: 14px; padding: 0.8rem 1.8rem; font-weight: 600;
            cursor: pointer; transition: all var(--transition); font-size: 0.95rem;
        }
        .btn:hover { transform: translateY(-2px); box-shadow: 0 8px 20px rgba(79,70,229,0.3); }
        .btn-danger { background: linear-gradient(135deg, #ef4444, #dc2626); }
        .btn-success { background: linear-gradient(135deg, #10b981, #059669); }
        .btn-warning { background: linear-gradient(135deg, #f59e0b, #d97706); }
        input, select {
            width: 100%; padding: 0.85rem 1.2rem; border: 1px solid var(--border);
            border-radius: 14px; background: var(--surface); color: var(--text); outline: none;
            transition: all var(--transition); margin-bottom: 0.8rem; font-family: inherit;
        }
        input:focus, select:focus { border-color: var(--primary); box-shadow: 0 0 0 3px rgba(79,70,229,0.15); }

        #loginPage {
            z-index: 2; width: 100%; max-width: 460px; background: rgba(255,255,255,0.75);
            backdrop-filter: blur(20px); border: 1px solid rgba(255,255,255,0.5);
            border-radius: 24px; padding: 3rem 2.5rem; text-align: center; position: relative;
        }
        .school-name { font-size: 2.4rem; font-weight: 700; background: linear-gradient(135deg, var(--primary), #ec4899); -webkit-background-clip: text; -webkit-text-fill-color: transparent; }
        .switch-form { margin-top: 1.5rem; color: var(--text-secondary); }
        .switch-form a { color: var(--primary); cursor: pointer; text-decoration: underline; font-weight: 600; }

        #appPage { display: none; position: relative; z-index: 2; width: 100%; max-width: 1400px; min-height: 90vh; background: var(--surface); backdrop-filter: blur(16px); border: 1px solid var(--border); border-radius: 24px; box-shadow: var(--shadow); overflow: hidden; }
        .app-container { display: flex; height: 100%; min-height: 90vh; }
        .sidebar { width: 280px; background: rgba(255,255,255,0.5); backdrop-filter: blur(20px); border-right: 1px solid var(--border); padding: 1.8rem 1.2rem; display: flex; flex-direction: column; }
        .sidebar-logo { display: flex; align-items: center; gap: 0.8rem; padding: 0.5rem 1rem; margin-bottom: 2rem; font-size: 1.4rem; font-weight: 700; background: linear-gradient(135deg, var(--primary), #ec4899); -webkit-background-clip: text; -webkit-text-fill-color: transparent; }
        .sidebar-item { display: flex; align-items: center; gap: 0.5rem; padding: 0.9rem 1.2rem; border-radius: 14px; cursor: pointer; transition: 0.2s; color: var(--text-secondary); margin-bottom: 0.2rem; font-weight: 500; }
        .sidebar-item:hover, .sidebar-item.active { background: linear-gradient(135deg, rgba(79,70,229,0.12), rgba(236,72,153,0.08)); color: var(--primary); font-weight: 600; }
        .main-area { flex: 1; overflow-y: auto; display: flex; flex-direction: column; }
        .top-header { display: flex; align-items: center; padding: 1rem 2rem; background: rgba(255,255,255,0.4); backdrop-filter: blur(10px); border-bottom: 1px solid var(--border); gap: 1.5rem; }
        .content-wrapper { padding: 2rem; flex: 1; }

        .dashboard-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(220px, 1fr)); gap: 1.5rem; margin-top: 1rem; }
        .subject-card { border-radius: 22px; padding: 2rem 1.5rem; cursor: pointer; transition: all 0.3s; color: white; box-shadow: 0 8px 25px rgba(0,0,0,0.08); }
        .subject-card:hover { transform: translateY(-8px); box-shadow: 0 20px 35px rgba(0,0,0,0.15); }
        .subject-card h3 { font-size: 1.5rem; font-weight: 600; }
        .subject-card .video-count { margin-top: 0.5rem; font-size: 0.9rem; opacity: 0.9; background: rgba(255,255,255,0.2); display: inline-block; padding: 0.3rem 1rem; border-radius: 20px; }

        .video-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.8rem; margin-top: 1rem; }
        .video-card { background: var(--surface); border-radius: 20px; overflow: hidden; box-shadow: var(--shadow); transition: 0.3s; border: 1px solid var(--border); }
        .video-card:hover { transform: translateY(-4px); box-shadow: 0 15px 30px rgba(0,0,0,0.1); }
        .video-card video { width: 100%; height: 200px; object-fit: cover; background: #000; }
        .video-info { padding: 1.2rem; }

        .modal { display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(15,23,42,0.6); backdrop-filter: blur(8px); align-items: center; justify-content: center; z-index: 1000; }
        .modal.active { display: flex; }
        .modal-content { background: var(--surface); border-radius: 24px; padding: 2.5rem; width: 90%; max-height: 90vh; overflow-y: auto; box-shadow: 0 30px 60px rgba(0,0,0,0.2); }
        .close-modal { float: right; font-size: 1.8rem; cursor: pointer; color: var(--text-secondary); transition: 0.2s; }
        .close-modal:hover { color: var(--text); transform: rotate(90deg); }
        .divider { border-top: 1px solid var(--border); margin: 1.5rem 0; }

        .stats-modal-large { max-width: 95% !important; }
        .stats-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 1.2rem; margin-bottom: 2rem; }
        .stat-card { background: var(--surface); border: 1px solid var(--border); border-radius: 16px; padding: 2rem; text-align: center; box-shadow: var(--shadow); }
        .stat-card .stat-number { font-size: 2.5rem; font-weight: 700; }
        .stat-card .stat-label { color: var(--text-secondary); font-size: 0.95rem; margin-top: 0.3rem; font-weight: 500; }
        .search-box { position: relative; margin-bottom: 1.5rem; }
        .search-box input { padding-left: 3rem; font-size: 1rem; }
        .search-box i { position: absolute; left: 1rem; top: 50%; transform: translateY(-50%); color: var(--text-secondary); }
        table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
        th, td { text-align: left; padding: 1rem; border-bottom: 1px solid var(--border); }
        th { color: var(--text-secondary); font-weight: 600; font-size: 0.9rem; text-transform: uppercase; }
        .badge { padding: 0.3rem 0.8rem; border-radius: 20px; font-size: 0.8rem; font-weight: 600; }
        .badge-active { background: rgba(16,185,129,0.15); color: #10b981; }
        .badge-expired { background: rgba(239,68,68,0.15); color: #ef4444; }
        .badge-never { background: rgba(148,163,184,0.15); color: #64748b; }
        .action-btns { display: flex; gap: 0.5rem; flex-wrap: wrap; }

        @media (max-width: 900px) {
            .app-container { flex-direction: column; }
            .sidebar { width: 100%; border-right: none; border-bottom: 1px solid var(--border); flex-direction: row; flex-wrap: wrap; padding: 1rem; }
            .sidebar-nav { display: flex; overflow-x: auto; gap: 0.5rem; flex: 1; }
            .sidebar-item { white-space: nowrap; padding: 0.6rem 1rem; }
            .sidebar-logo { width: 100%; }
            .stats-modal-large { max-width: 100% !important; }
        }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/@supabase/supabase-js@2"></script>



    


        

🎓


        

Enoneno's School


        

A plataforma que ilumina o teu conhecimento


        


            Email
            @email.com" autocomplete="email">
            Palavra-passe
            
            

Email ou palavra-passe incorretos.


             Entrar
            

Não tem conta? Criar uma


        
        


            Nome completo
            
            Email
            @email.com">
            Palavra-passe
            
            Confirmar
            
            
            

Conta criada! Verifica o teu email (pode estar no spam).


             Criar conta
            

Já tem conta? Entrar


        
    

    


        


            


                

🎓 Enoneno's


                


                


                    

U


                    

Aluno
email


                


            


            


                


                    📚 Disciplinas
                    


                    


                         Admin
                         Estatísticas
                        
                        
                    


                


                


            


        


    



    


        


            ×
            

 Painel de Administração


            

Criar Sala


            
             Criar Sala
            


            

Remover Sala


            Escolha uma sala...
             Eliminar Sala
            


            

Adicionar Vídeo


            Escolha uma sala...
            
            
             Adicionar Vídeo
            


            

Alterar Palavra-passe Admin


            
             Alterar
            


        


    



    


        


            

💰


            

Pagamento Necessário


            

Para aceder às salas, efetue o pagamento de 300 MZN (válido por 2 meses).


             Pagar 300 MZN
            

(Pagamento simulado – ambiente de teste)


        


    



    


        


            ×
            

 Estatísticas & Gestão de Alunos

This project was built with [Lovable](https://lovable.dev).

## Build with Lovable

Continue developing this project in the [Lovable editor](https://lovable.dev/projects/f491b393-2d2f-4bdb-8bc3-02e9821e39ba).

- **Ship faster**: describe what you want to build and Lovable handles the code.
- **Stay in sync**: every change made in Lovable is committed straight to this repository.
- **Full ownership**: this code is yours. Push to `main` on GitHub and your changes sync back into Lovable, ready for your next prompt.

## Development

Prefer working locally? You need Node.js and npm — [install with nvm](https://github.com/nvm-sh/nvm#installing-and-updating).

```sh
git clone <this-repository-url>
cd <repository-name>
npm i
npm run dev
```
