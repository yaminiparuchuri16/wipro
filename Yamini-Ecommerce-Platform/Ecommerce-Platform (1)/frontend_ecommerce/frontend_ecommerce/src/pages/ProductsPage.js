import React from 'react';
import { useSelector } from 'react-redux';
import ProductCard from '../components/ProductCard';

export default function ProductsPage() {
  // Combine static and API products for full list
  const items = useSelector(s => s.products.items);
  const staticProducts = [
    { id: 101, name: 'Apple iPhone 15', description: 'Latest iPhone with A16 Bionic chip', price: 79999, category: 'Mobile', imageUrl: 'https://m.media-amazon.com/images/I/71d7rfSl0wL._SX679_.jpg' },
    { id: 102, name: 'Samsung Galaxy S23', description: 'Flagship Android phone', price: 69999, category: 'Mobile', imageUrl: 'https://m.media-amazon.com/images/I/81ZSn2rk9WL._SX679_.jpg' },
    { id: 103, name: 'Sony WH-1000XM5', description: 'Noise Cancelling Headphones', price: 29999, category: 'Audio', imageUrl: 'https://m.media-amazon.com/images/I/41JACWT-wWL._SX679_.jpg' },
    { id: 104, name: 'Apple MacBook Air M2', description: 'Ultra-thin laptop with M2 chip', price: 109999, category: 'Laptop', imageUrl: 'https://m.media-amazon.com/images/I/71jG+e7roXL._SX679_.jpg' },
    { id: 105, name: 'Canon EOS R10', description: 'Mirrorless Camera for creators', price: 84999, category: 'Camera', imageUrl: 'https://m.media-amazon.com/images/I/71eOQ5ssTAL._SX679_.jpg' }
  ];

  // Load products from localStorage
  const localProducts = (() => {
    try {
      const arr = JSON.parse(localStorage.getItem('products'));
      return Array.isArray(arr) ? arr : [];
    } catch {
      return [];
    }
  })();

  const allProducts = [...staticProducts, ...items, ...localProducts];
  return (
    <div>
      <div style={{
        display: 'flex',
        justifyContent: 'center',
        marginBottom: '18px',
      }}>
        <div style={{
          background: 'linear-gradient(90deg, #1e3a8a 0%, #b7e7a7 60%, #a75a7c 100%)',
          color: '#fff',
          borderRadius: '12px',
          padding: '12px 32px',
          fontWeight: 'bold',
          fontSize: '2.5rem',
          boxShadow: '0 2px 12px #b7e7a7',
          display: 'inline-block',
        }}>
          <span role="img" aria-label="shop">🛍️</span> E-Shop
        </div>
      </div>
      <h2 style={{color:'#2874f0',fontWeight:'bold',marginBottom:16}}>All Products</h2>
      <div className="grid" style={{display:'grid',gridTemplateColumns:'repeat(auto-fit,minmax(260px,1fr))',gap:24}}>
        {allProducts.map((p, idx) => <ProductCard key={p.id || p._id || idx} p={p} />)}
      </div>
    </div>
  );
}
