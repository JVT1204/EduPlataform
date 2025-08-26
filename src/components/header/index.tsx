import Link from "next/link";

export default function Header() {
  return (
    <header className="bg-gray-800 p-4 flex justify-between items-center text-white">
      <h1>EduPlataform</h1>
      <nav>
        <ul className="flex gap-4">
          <li>
            <Link href="/">Home</Link>
          </li>
          <li>
            <Link href="/dashbord">Dashbord</Link>
          </li>

          <li>
            <Link href="/contato">Contato</Link>
          </li>
        </ul>
      </nav>
    </header>
  );
}
