"use client";

import Link from "next/link";
import { useEffect } from "react";
import { getCurrentUser, logout, isAuthenticated } from "@/lib/auth";

export default function AdminLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const user = getCurrentUser();

  useEffect(() => {
    if (!isAuthenticated()) {
      window.location.href = "/login";
    } else if (user && user.role !== "Admin" && user.role !== "Teacher") {
      window.location.href = "/dashboard";
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (!user || (user.role !== "Admin" && user.role !== "Teacher")) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <div className="text-center">
          <p className="text-gray-500">Sem permissões de administração.</p>
          <Link
            href="/dashboard"
            className="mt-4 inline-block text-blue-600 underline"
          >
            Voltar
          </Link>
        </div>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <div className="flex items-center gap-6">
            <h1 className="text-lg font-bold text-gray-900">Administração</h1>
            <nav className="flex gap-4 text-sm">
              <Link
                href="/admin/courses"
                className="text-gray-600 hover:text-gray-900"
              >
                Cursos
              </Link>
              <Link
                href="/dashboard"
                className="text-gray-600 hover:text-gray-900"
              >
                Ver plataforma
              </Link>
            </nav>
          </div>
          <button
            onClick={() => void logout()}
            className="rounded-lg border border-gray-300 px-3 py-1.5 text-sm text-gray-600 hover:bg-gray-100"
          >
            Sair
          </button>
        </div>
      </header>
      <div className="mx-auto max-w-6xl px-6 py-8">{children}</div>
    </main>
  );
}