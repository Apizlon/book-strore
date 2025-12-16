export function Footer() {
  return (
    <footer className="bg-gray-900 text-white mt-12 py-8">
      <div className="max-w-7xl mx-auto px-4">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div>
            <h3 className="font-bold text-lg mb-4">About BookStore</h3>
            <p className="text-gray-400">
              Your favorite online bookstore with thousands of titles.
            </p>
          </div>
          <div>
            <h3 className="font-bold text-lg mb-4">Quick Links</h3>
            <ul className="text-gray-400 space-y-2">
              <li><a href="#" className="hover:text-white">About Us</a></li>
              <li><a href="#" className="hover:text-white">Contact</a></li>
              <li><a href="#" className="hover:text-white">Terms</a></li>
            </ul>
          </div>
          <div>
            <h3 className="font-bold text-lg mb-4">Contact</h3>
            <p className="text-gray-400">Email: info@bookstore.com</p>
            <p className="text-gray-400">Phone: +1-234-567-8900</p>
          </div>
        </div>
        <div className="border-t border-gray-700 mt-8 pt-8 text-center text-gray-400">
          <p>&copy; 2025 BookStore. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
}
