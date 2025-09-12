import React from 'react';
import { Link } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { addToCart } from '../redux/slices/cartSlice';

export default function ProductCard({ p }) {
  const dispatch = useDispatch();
  const [showMsg, setShowMsg] = React.useState(false);
  // Add to cart using frontend only
  const handleAddToCart = () => {
    dispatch({
      type: 'cart/addToCartFrontend',
      payload: { productId: p.id, quantity: 1, name: p.name, price: p.price, imageUrl: p.imageUrl }
    });
    setShowMsg(true);
    setTimeout(() => setShowMsg(false), 1500);
  };
  return (
    <div className="card product-card" style={{transition:'box-shadow .2s',border:'1px solid #e6e9ee',position:'relative',boxShadow:'0 2px 8px #f0f0f0',borderRadius:8}}>
      <img src={p.imageUrl || `https://picsum.photos/400/300?random=${p.id}`} alt={p.name} style={{borderRadius:'8px 8px 0 0',height:180,objectFit:'cover'}} />
      <div className="product-title" style={{fontWeight:'bold',fontSize:18,margin:'8px 0'}}>{p.name}</div>
      <div className="product-desc" style={{color:'#555',fontSize:14}}>{p.description}</div>
      <div className="price" style={{color:'#2874f0',fontWeight:'bold',fontSize:16}}>₹{p.price}</div>
      <div style={{display:'flex',gap:8,marginTop:12}}>
        <button className="button flipkart-btn" style={{background:'#ff9f00',color:'#fff',border:'none',borderRadius:4,padding:'8px 16px',fontWeight:'bold',boxShadow:'0 1px 4px #e0e0e0'}} onClick={handleAddToCart}>Add to Cart</button>
        <Link to={'/product/'+p.id}><button className="button secondary" style={{background:'#2874f0',color:'#fff',border:'none',borderRadius:4,padding:'8px 16px',fontWeight:'bold'}}>View Details</button></Link>
      </div>
      {showMsg && (
        <div style={{
          position: 'absolute',
          top: 16,
          left: '50%',
          transform: 'translateX(-50%)',
          background: 'rgba(40,116,240,0.95)',
          color: '#fff',
          padding: '14px 32px',
          borderRadius: '16px',
          fontWeight: 'bold',
          fontSize: '1.1rem',
          boxShadow: '0 4px 24px #2874f0',
          zIndex: 100,
          letterSpacing: '1px',
          border: '2px solid #b7e7a7',
          textAlign: 'center',
          animation: 'fadeInOut 1.5s',
        }}>
          <span role="img" aria-label="cart">🛒</span> Added to cart successfully!
        </div>
      )}
    </div>
  );
}
