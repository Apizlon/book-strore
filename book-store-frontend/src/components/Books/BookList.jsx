import { useState, useEffect } from 'react';
import { booksAPI } from '../../api/books';
import { BookCard } from './BookCard';
import { BOOK_GENRES } from '../../utils/constants';

export function BookList() {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedGenre, setSelectedGenre] = useState('');
  const [sortBy, setSortBy] = useState('new');

  useEffect(() => {
    fetchBooks();
  }, [selectedGenre, sortBy]);

  const fetchBooks = async () => {
    setLoading(true);
    try {
      let response;
      if (selectedGenre) {
        response = await booksAPI.getBooksByGenre(selectedGenre);
      } else if (sortBy === 'rating') {
        response = await booksAPI.getTopRatedBooks(20);
      } else {
        response = await booksAPI.getAllBooks();
      }
      setBooks(response.data);
    } catch (error) {
      console.error('Failed to fetch books:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-7xl mx-auto px-4 py-8">
      <h1 className="text-4xl font-bold mb-8">Our Books</h1>

      <div className="flex flex-col md:flex-row gap-4 mb-8">
        <select
          value={selectedGenre}
          onChange={(e) => setSelectedGenre(e.target.value)}
          className="px-4 py-2 border rounded-lg"
        >
          <option value="">All Genres</option>
          {BOOK_GENRES.map(genre => (
            <option key={genre} value={genre}>{genre}</option>
          ))}
        </select>

        <select
          value={sortBy}
          onChange={(e) => setSortBy(e.target.value)}
          className="px-4 py-2 border rounded-lg"
        >
          <option value="new">Newest</option>
          <option value="rating">Top Rated</option>
        </select>
      </div>

      {loading ? (
        <div className="text-center py-12">Loading...</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {books.map(book => (
            <BookCard key={book.id} book={book} />
          ))}
        </div>
      )}
    </div>
  );
}
