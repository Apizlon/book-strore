import { Link } from 'react-router-dom';
import { ArrowRight, BookOpen, Users, Zap } from 'lucide-react';

export function Home() {
  return (
    <div style={{ minHeight: '100vh' }}>
      {/* Hero Section */}
      <section style={{
        background: 'linear-gradient(to right, #208092, #1a6b7f, #32b8c6)',
        color: 'white',
        padding: '96px 20px',
        textAlign: 'center'
      }}>
        <div style={{ maxWidth: '1280px', margin: '0 auto' }}>
          <h1 style={{
            fontSize: '48px',
            fontWeight: 'bold',
            marginBottom: '24px',
            lineHeight: '1.2'
          }}>
            Welcome to <span style={{ color: '#32b8c6' }}>BookStore</span>
          </h1>
          <p style={{
            fontSize: '18px',
            marginBottom: '32px',
            opacity: 0.95,
            maxWidth: '640px',
            margin: '0 auto 32px',
            lineHeight: '1.6'
          }}>
            Discover your next favorite book from thousands of titles across all genres
          </p>
          <Link
            to="/books"
            style={{
              background: 'white',
              color: '#208092',
              padding: '16px 32px',
              borderRadius: '12px',
              fontWeight: 'bold',
              fontSize: '16px',
              boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.1)',
              display: 'inline-flex',
              alignItems: 'center',
              gap: '12px',
              textDecoration: 'none',
              transition: 'all 0.3s'
            }}
          >
            Start Browsing <ArrowRight style={{ width: '20px', height: '20px' }} />
          </Link>
        </div>
      </section>

      {/* Features Section */}
      <section style={{
        padding: '80px 20px',
        background: '#fff'
      }}>
        <div style={{ maxWidth: '1280px', margin: '0 auto' }}>
          <h2 style={{
            fontSize: '40px',
            fontWeight: 'bold',
            textAlign: 'center',
            marginBottom: '80px',
            color: '#1f2937'
          }}>
            Why Choose Us
          </h2>
          
          <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))',
            gap: '32px'
          }}>
            {/* Card 1 */}
            <div style={{
              textAlign: 'center',
              padding: '40px',
              borderRadius: '20px',
              background: 'linear-gradient(135deg, #f0fafb 0%, #e0f7ff 100%)',
              border: '1px solid #d0e8ed',
              boxShadow: '0 4px 15px rgba(32, 128, 146, 0.1)',
              transition: 'all 0.3s'
            }}>
              <BookOpen style={{
                width: '60px',
                height: '60px',
                margin: '0 auto 24px',
                color: '#208092'
              }} />
              <h3 style={{
                fontSize: '20px',
                fontWeight: 'bold',
                marginBottom: '16px',
                color: '#1f2937'
              }}>
                Wide Selection
              </h3>
              <p style={{
                fontSize: '15px',
                color: '#6b7280',
                lineHeight: '1.6',
                maxWidth: '300px',
                margin: '0 auto'
              }}>
                Browse thousands of books across all genres from classic literature to modern bestsellers
              </p>
            </div>

            {/* Card 2 */}
            <div style={{
              textAlign: 'center',
              padding: '40px',
              borderRadius: '20px',
              background: 'linear-gradient(135deg, #f0faf8 0%, #e0f8f5 100%)',
              border: '1px solid #d0e8e3',
              boxShadow: '0 4px 15px rgba(32, 128, 146, 0.1)',
              transition: 'all 0.3s'
            }}>
              <Users style={{
                width: '60px',
                height: '60px',
                margin: '0 auto 24px',
                color: '#208092'
              }} />
              <h3 style={{
                fontSize: '20px',
                fontWeight: 'bold',
                marginBottom: '16px',
                color: '#1f2937'
              }}>
                Community Reviews
              </h3>
              <p style={{
                fontSize: '15px',
                color: '#6b7280',
                lineHeight: '1.6',
                maxWidth: '300px',
                margin: '0 auto'
              }}>
                Read honest reviews from fellow book lovers and share your own reading experiences
              </p>
            </div>

            {/* Card 3 */}
            <div style={{
              textAlign: 'center',
              padding: '40px',
              borderRadius: '20px',
              background: 'linear-gradient(135deg, #f8f5fa 0%, #f0e8ff 100%)',
              border: '1px solid #e0d0e8',
              boxShadow: '0 4px 15px rgba(32, 128, 146, 0.1)',
              transition: 'all 0.3s'
            }}>
              <Zap style={{
                width: '60px',
                height: '60px',
                margin: '0 auto 24px',
                color: '#208092'
              }} />
              <h3 style={{
                fontSize: '20px',
                fontWeight: 'bold',
                marginBottom: '16px',
                color: '#1f2937'
              }}>
                Fast Checkout
              </h3>
              <p style={{
                fontSize: '15px',
                color: '#6b7280',
                lineHeight: '1.6',
                maxWidth: '300px',
                margin: '0 auto'
              }}>
                Quick and secure purchase process with multiple payment options
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section style={{
        background: 'linear-gradient(to right, #208092, #1a6b7f, #32b8c6)',
        padding: '96px 20px',
        textAlign: 'center',
        color: 'white'
      }}>
        <div style={{ maxWidth: '960px', margin: '0 auto' }}>
          <h2 style={{
            fontSize: '40px',
            fontWeight: 'bold',
            marginBottom: '32px',
            textShadow: '0 2px 4px rgba(0, 0, 0, 0.2)'
          }}>
            Ready to Find Your Next Read?
          </h2>
          <p style={{
            fontSize: '18px',
            marginBottom: '32px',
            opacity: 0.95,
            lineHeight: '1.6'
          }}>
            Join thousands of satisfied customers who found their perfect books here
          </p>
          <Link
            to="/books"
            style={{
              background: 'white',
              color: '#208092',
              padding: '16px 32px',
              fontSize: '16px',
              fontWeight: 'bold',
              borderRadius: '16px',
              boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.1)',
              display: 'inline-flex',
              alignItems: 'center',
              gap: '12px',
              textDecoration: 'none',
              transition: 'all 0.3s'
            }}
          >
            Browse All Books <ArrowRight style={{ width: '20px', height: '20px' }} />
          </Link>
        </div>
      </section>
    </div>
  );
}
