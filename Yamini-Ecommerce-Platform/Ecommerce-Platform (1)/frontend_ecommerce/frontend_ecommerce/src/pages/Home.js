import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchProducts } from '../redux/slices/productSlice';
import ProductCard from '../components/ProductCard';


export default function Home(){
  const dispatch = useDispatch();
  const items = useSelector(s => s.products.items);
  // Static products for frontend only
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

  useEffect(()=>{ dispatch(fetchProducts()); }, [dispatch]);

  // Merge static, API, and localStorage products
  const allProducts = [...staticProducts, ...items, ...localProducts];

  // Extract categories
  const categories = Array.from(new Set(allProducts.map(p => p.category).filter(Boolean)));

  // State for filters
  const [selectedCategory, setSelectedCategory] = React.useState('');
  const [sortOrder, setSortOrder] = React.useState('');
  const [search, setSearch] = React.useState('');

  // Filter and sort products
  let filtered = allProducts.filter(p => {
    const searchText = search.toLowerCase();
    const matches =
      (!search ||
        (p.name && p.name.toLowerCase().includes(searchText)) ||
        (p.description && p.description.toLowerCase().includes(searchText)) ||
        (p.category && p.category.toLowerCase().includes(searchText)));
    return (!selectedCategory || p.category === selectedCategory) && matches;
  });
  if (sortOrder === 'low') filtered = filtered.slice().sort((a,b)=>a.price-b.price);
  if (sortOrder === 'high') filtered = filtered.slice().sort((a,b)=>b.price-a.price);

  // Convert INR to USD (approx, for demo)
  const INR_TO_USD = 0.012;
  const formatUSD = inr => `$${(inr * INR_TO_USD).toFixed(2)}`;

  return (
    <div>
      <div 
        className="eshop-welcome"
        style={{
          background: 'linear-gradient(90deg, #2874f0 0%, #5fe61cc0 100%)',
          color: '#fff',
          padding: '40px 0 32px 0',
          borderRadius: 16,
          marginBottom: 40,
          textAlign: 'center',
          boxShadow: '0 4px 24px #e3e3e3' ,
          transition: 'background 0.6s, box-shadow 0.3s',
          cursor: 'pointer',
          position: 'relative',
          overflow: 'hidden'
        }}
        onMouseEnter={e => {
          e.currentTarget.style.background = 'linear-gradient(90deg, #00c6ff 0%, #2874f0 100%)';
          e.currentTarget.style.boxShadow = '0 8px 32px #b3e0ff';
        }}
        onMouseLeave={e => {
          e.currentTarget.style.background = 'linear-gradient(90deg, #2874f0 0%, #00c6ff 100%)';
          e.currentTarget.style.boxShadow = '0 4px 24px #e3e3e3';
        }}
      >
        <div style={{
          background: 'linear-gradient(90deg, #1e3a8a 0%, #b7e7a7 60%, #a75a7c 100%)',
          color: '#fff',
          borderRadius: '12px',
          padding: '12px 32px',
          fontWeight: 'bold',
          fontSize: '2.5rem',
          boxShadow: '0 2px 12px #b7e7a7',
          display: 'inline-block',
          marginBottom: '18px',
        }}>
          <span role="img" aria-label="shop">🛍️</span> Welcome to E-Shop
        </div>
        <p style={{fontSize:20,marginBottom:16,opacity:0.95}}>Your one-stop destination for the latest electronics, gadgets, and unbeatable deals.</p>
        <div style={{fontSize:16,margin:'0 auto',maxWidth:600,opacity:0.92}}>
          <ul style={{listStyle:'none',padding:0,margin:0,display:'flex',justifyContent:'center',gap:32}}>
            <li style={{display:'flex',alignItems:'center',gap:8}}><span role="img" aria-label="shipping">🚚</span> Free Fast Shipping</li>
            <li style={{display:'flex',alignItems:'center',gap:8}}><span role="img" aria-label="support">💬</span> 24/7 Support</li>
            <li style={{display:'flex',alignItems:'center',gap:8}}><span role="img" aria-label="secure">🔒</span> Secure Payments</li>
          </ul>
        </div>
      </div>
      {/* Filters and search */}
      <div style={{display:'flex',gap:16,marginBottom:24,alignItems:'center',flexWrap:'wrap'}}>
        <select className="input" value={selectedCategory} onChange={e=>setSelectedCategory(e.target.value)} style={{minWidth:140}}>
          <option value="">All Categories</option>
          {categories.map(c => <option key={c} value={c}>{c}</option>)}
        </select>
        <select className="input" value={sortOrder} onChange={e=>setSortOrder(e.target.value)} style={{minWidth:140}}>
          <option value="">Sort by Price</option>
          <option value="low">Low to High (USD)</option>
          <option value="high">High to Low (USD)</option>
        </select>
        <input className="input" style={{minWidth:180}} type="text" placeholder="Search products..." value={search} onChange={e=>setSearch(e.target.value)} />
      </div>
      <h2 style={{color:'#2874f0',fontWeight:'bold',marginBottom:16}}>Products</h2>
      <div className="grid" style={{display:'grid',gridTemplateColumns:'repeat(auto-fit,minmax(260px,1fr))',gap:24}}>
        {filtered.map((p, idx) => <ProductCard key={p.id || p._id || idx} p={{...p, priceUSD: formatUSD(p.price)}} />)}
      </div>
    </div>
  );
}
