import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { useEffect, useState, useCallback } from 'react';
import { Header } from './components/Layout/Header';
import { Footer } from './components/Layout/Footer';
import { Home } from './pages/Home';
import { BookList } from './components/Books/BookList';
import { BookDetail } from './components/Books/BookDetail';
import { CartPage } from './components/Cart/CartPage';
import { OrdersPage } from './components/Orders/OrdersPage';  // ✅ НОВОЕ
import { ProfilePage } from './components/Profile/ProfilePage'; // ✅ НОВОЕ
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';

function App() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const loadUser = useCallback(() => {
    console.log('🔄 App: Loading user from localStorage...');
    
    const userData = localStorage.getItem('user');
    const token = localStorage.getItem('token');
    
    console.log('📦 Raw localStorage user:', userData);
    console.log('🔑 localStorage token:', token ? 'EXISTS' : 'MISSING');

    if (userData === 'undefined' || userData === 'null') {
      console.log('🗑️ Clearing invalid localStorage data');
      localStorage.removeItem('user');
      localStorage.removeItem('token');
      setUser(null);
      setLoading(false);
      return;
    }

    if (userData && token) {
      try {
        const parsedUser = JSON.parse(userData);
        console.log('✅ App: User LOADED:', parsedUser.username || parsedUser);
        setUser(parsedUser);
      } catch (e) {
        console.error('❌ App: Parse FAILED:', e);
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        setUser(null);
      }
    } else {
      console.log('❌ No valid user data');
      setUser(null);
    }
    setLoading(false);
  }, []);

  useEffect(() => {
    loadUser();
  }, [loadUser]);

  const handleLoginSuccess = () => {
    console.log('🎉 App: Login success callback');
    loadUser();
  };

  if (loading) {
    return <div style={{ padding: '50px', textAlign: 'center' }}>Loading...</div>;
  }

  const isAuthenticated = !!user && !!localStorage.getItem('token');

  return (
    <Router>
      <div style={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
        <Header user={user} />
        <main style={{ flex: 1 }}>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/books" element={<BookList />} />
            <Route path="/books/:id" element={<BookDetail />} />
            <Route path="/profile" element={isAuthenticated ? <ProfilePage /> : <Navigate to="/login" replace />} />
            <Route path="/orders" element={isAuthenticated ? <OrdersPage /> : <Navigate to="/login" replace />} />
            <Route path="/cart" element={isAuthenticated ? <CartPage /> : <Navigate to="/login" replace />} />
            <Route path="/login" element={<LoginPage onLoginSuccess={handleLoginSuccess} />} />
            <Route path="/register" element={<RegisterPage />} />
          </Routes>
        </main>
        <Footer />
      </div>
    </Router>
  );
}

export default App;
