
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from '../services/api';
import { useDispatch, useSelector } from 'react-redux';
import { addToCart } from '../redux/slices/cartSlice';

export default function ProductDetails() {
  const { id } = useParams();
  const dispatch = useDispatch();
  const items = useSelector(s => s.products.items);
  // Static products for frontend only
  const staticProducts = [
    { id: '1', name: 'Laptop', price: 899.99, description: 'High performance laptop', imageUrl: '' },
    { id: '2', name: 'Phone', price: 499.99, description: 'Latest smartphone', imageUrl: '' },
    { id: '3', name: 'Tablet', price: 299.99, description: 'Portable tablet', imageUrl: '' },
    // ...add more static products if needed
  ];
  const allProducts = [...staticProducts, ...items];
  const found = allProducts.find(p => p.id === id || p._id === id);
  const [p, setP] = useState(found || null);

  useEffect(() => {
    if (!p) {
      api.get('/Products/' + id).then(r => setP(r.data)).catch(() => {});
    }
  }, [id, p]);

  if (!p) return <div className="center">Loading...</div>;

  return (
    <div style={{ display: 'flex', gap: 20, alignItems: 'flex-start' }}>
      <div style={{ flex: 2 }} className="card">
        <img src={p.imageUrl || 'https://picsum.photos/800/500?random=' + (p.id || p._id)} alt={p.name} style={{ width: '100%', height: 320, objectFit: 'cover', borderRadius: 8 }} />
        <h3>{p.name}</h3>
        <p className="product-desc">{p.description}</p>
        <div className="price">₹{p.price}</div>
      </div>
      <aside style={{ width: 320 }} className="card aside-cart">
        <h4>Buy</h4>
        <p>Price: <strong>₹{p.price}</strong></p>
        <button className="button" onClick={() => dispatch(addToCart({ productId: p.id || p._id, name: p.name, unitPrice: p.price, quantity: 1 }))}>Add to cart</button>
      </aside>
    </div>
  );
}
