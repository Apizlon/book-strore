import { Link } from 'react-router-dom';
import { ArrowRight, BookOpen, Users, Zap } from 'lucide-react';

export function Home() {
  return (
    <div className="min-h-screen">
      {/* Hero Section */}
      <section className="bg-gradient-to-r from-primary-500 via-primary-600 to-accent-500 text-white py-24 px-4">
        <div className="max-w-7xl mx-auto text-center">
          <h1 className="text-5xl md:text-6xl font-bold mb-6 leading-tight">
            Welcome to <span className="text-accent-500">BookStore</span>
          </h1>
          <p className="text-xl md:text-2xl mb-12 opacity-95 max-w-2xl mx-auto leading-relaxed">
            Discover your next favorite book from thousands of titles across all genres
          </p>
          <Link
            to="/books"
            className="bg-white text-primary-500 px-8 py-4 rounded-xl font-bold text-lg shadow-2xl hover:shadow-3xl hover:-translate-y-1 transition-all duration-300 inline-flex items-center gap-3"
          >
            Start Browsing <ArrowRight className="w-5 h-5 group-hover:translate-x-1 transition-transform" />
          </Link>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-20 px-4 bg-white">
        <div className="max-w-7xl mx-auto">
          <h2 className="text-4xl md:text-5xl font-bold text-center mb-20 text-gray-900">
            Why Choose Us
          </h2>
          <div className="grid md:grid-cols-3 gap-12">
            <div className="group text-center p-10 rounded-3xl bg-gradient-to-br from-gray-50 to-blue-50 hover:shadow-2xl hover:-translate-y-3 transition-all duration-500 border border-gray-100">
              <BookOpen className="w-20 h-20 mx-auto mb-8 text-primary-500 group-hover:scale-110 group-hover:rotate-3 transition-all duration-500" />
              <h3 className="text-2xl md:text-3xl font-bold mb-6 text-gray-900">Wide Selection</h3>
              <p className="text-lg text-gray-600 max-w-md mx-auto leading-relaxed">
                Browse thousands of books across all genres from classic literature to modern bestsellers
              </p>
            </div>
            
            <div className="group text-center p-10 rounded-3xl bg-gradient-to-br from-gray-50 to-green-50 hover:shadow-2xl hover:-translate-y-3 transition-all duration-500 border border-gray-100">
              <Users className="w-20 h-20 mx-auto mb-8 text-primary-500 group-hover:scale-110 group-hover:rotate-3 transition-all duration-500" />
              <h3 className="text-2xl md:text-3xl font-bold mb-6 text-gray-900">Community Reviews</h3>
              <p className="text-lg text-gray-600 max-w-md mx-auto leading-relaxed">
                Read honest reviews from fellow book lovers and share your own reading experiences
              </p>
            </div>
            
            <div className="group text-center p-10 rounded-3xl bg-gradient-to-br from-gray-50 to-purple-50 hover:shadow-2xl hover:-translate-y-3 transition-all duration-500 border border-gray-100">
              <Zap className="w-20 h-20 mx-auto mb-8 text-primary-500 group-hover:scale-110 group-hover:rotate-3 transition-all duration-500" />
              <h3 className="text-2xl md:text-3xl font-bold mb-6 text-gray-900">Fast Checkout</h3>
              <p className="text-lg text-gray-600 max-w-md mx-auto leading-relaxed">
                Quick and secure purchase process with multiple payment options
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="bg-gradient-to-r from-primary-500 via-primary-600 to-accent-500 py-24 px-4">
        <div className="max-w-4xl mx-auto text-center text-white">
          <h2 className="text-4xl md:text-5xl font-bold mb-8 drop-shadow-lg">
            Ready to Find Your Next Read?
          </h2>
          <p className="text-xl md:text-2xl mb-12 opacity-95 drop-shadow-md">
            Join thousands of satisfied customers who found their perfect books here
          </p>
          <Link
            to="/books"
            className="bg-white text-primary-500 px-12 py-5 text-xl font-bold rounded-2xl shadow-2xl hover:shadow-3xl hover:-translate-y-2 transition-all duration-300 inline-flex items-center gap-3"
          >
            Browse All Books <ArrowRight className="w-6 h-6 group-hover:translate-x-2 transition-transform" />
          </Link>
        </div>
      </section>
    </div>
  );
}
