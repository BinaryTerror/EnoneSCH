import Link from "next/link";

export default function AdminHome() {
  return (
    <div>
      <h2 className="text-2xl font-bold text-gray-900">Painel de administração</h2>
      <p className="mt-1 text-gray-500">
        Gerir cursos, anos, semestres, disciplinas e aulas.
      </p>
      <div className="mt-8">
        <Link
          href="/admin/courses"
          className="inline-block rounded-xl border border-gray-200 bg-white p-6 shadow-sm hover:shadow-md"
        >
          <div className="text-3xl">📚</div>
          <div className="mt-2 font-semibold text-gray-900">Gerir Cursos</div>
          <div className="text-sm text-gray-500">
            Criar, editar e remover cursos e conteúdos.
          </div>
        </Link>
      </div>
    </div>
  );
}