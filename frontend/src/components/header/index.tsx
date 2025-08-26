import Link from "next/link";

export default function Header() {
  return (
    <header className="flex justify-between bg-gray-800 p-4">
      <h1 className="text-white">EduPlataform</h1>

      <ul className="flex gap-8">
        <li>
          <Link href="/" className="text-white hover:text-gray-200 uppercase">
            home
          </Link>
        </li>
        <li>
          <Link
            href="/about"
            className="text-white hover:text-gray-200 uppercase"
          >
            about
          </Link>
        </li>
        <li>
          <Link
            href="/contact"
            className="text-white hover:text-gray-200 uppercase"
          >
            contact
          </Link>
        </li>
      </ul>
    </header>
  );
}
