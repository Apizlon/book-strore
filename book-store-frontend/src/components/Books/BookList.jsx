import { useState, useEffect } from 'react';
import { BookCard } from './BookCard';

export function BookList() {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);

  useEffect(() => {
    fetchBooks();
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        setUser(JSON.parse(storedUser));
      } catch (e) {
        console.error('Failed to parse user:', e);
      }
    }
  }, []);

  const fetchBooks = async () => {
    try {
      const response = await fetch('http://localhost:5003/api/books');
      if (!response.ok) throw new Error('Failed to fetch books');
      const data = await response.json();
      setBooks(data);
    } catch (err) {
      console.error('Books error:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div style={{ padding: '100px', textAlign: 'center' }}>Loading books...</div>;

  return (
    <div style={{ padding: '40px 20px', maxWidth: '1400px', margin: '0 auto' }}>
      <h1 style={{ fontSize: '36px', fontWeight: 'bold', textAlign: 'center', marginBottom: '40px', color: '#1f2937' }}>
        📚 Our Books Collection
      </h1>
      
      <div style={{ 
        display: 'grid', 
        gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', 
        gap: '30px' 
      }}>
        {books.map(book => (
          <BookCard key={book.id} book={book} user={user} />
        ))}
      </div>
    </div>
  );
}
