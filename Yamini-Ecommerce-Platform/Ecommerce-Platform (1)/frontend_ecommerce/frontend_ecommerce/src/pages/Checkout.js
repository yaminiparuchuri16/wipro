import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { placeOrder } from '../redux/slices/orderSlice';
import { clearCart } from '../redux/slices/cartSlice';
import { useNavigate } from 'react-router-dom';

export default function Checkout(){
  const cart = useSelector(s => s.cart.items);
  const user = useSelector(s => s.auth.user);
  const dispatch = useDispatch();
  const nav = useNavigate();
  const [address, setAddress] = useState('');
  const [contact, setContact] = useState('');
  const [showOrderMsg, setShowOrderMsg] = useState(false);

  const total = cart.reduce((s,i)=>s + i.unitPrice * i.quantity, 0);

  const doPlace = async ()=>{
    // Create order object
    const order = {
      id: Date.now(),
      user: user ? user.email : 'Guest',
      items: cart.map(i=>({ name: i.name, quantity: i.quantity })),
      total,
      status: 'Pending'
    };
    // Save to localStorage
    const orders = JSON.parse(localStorage.getItem('orders') || '[]');
    orders.push(order);
    localStorage.setItem('orders', JSON.stringify(orders));
    dispatch(clearCart());
    setShowOrderMsg(true);
    setTimeout(() => {
      setShowOrderMsg(false);
      nav('/orders');
    }, 1800);
  };

  return (
    <div style={{display:'flex',gap:20,position:'relative'}}>
      {showOrderMsg && (
        <div style={{
          position: 'absolute',
          top: 24,
          left: '50%',
          transform: 'translateX(-50%)',
          background: 'rgba(40,116,240,0.95)',
          color: '#fff',
          padding: '16px 36px',
          borderRadius: '18px',
          fontWeight: 'bold',
          fontSize: '1.2rem',
          boxShadow: '0 4px 24px #2874f0',
          zIndex: 100,
          letterSpacing: '1px',
          border: '2px solid #a75a7c',
          textAlign: 'center',
          animation: 'fadeInOut 1.8s',
        }}>
          <span role="img" aria-label="order">✅</span> Order placed successfully!
        </div>
      )}
      <div style={{flex:2}}>
        <h3>Checkout</h3>
        <div className="card">
          <div className="form-row"><input className="input" placeholder="Full name" /></div>
          <div className="form-row"><input className="input" placeholder="Address" value={address} onChange={e=>setAddress(e.target.value)} /></div>
          <div className="form-row"><input className="input" placeholder="Contact number" value={contact} onChange={e=>setContact(e.target.value)} /></div>
          <p className="small">Payment: <strong>Stripe / PayPal integration placeholder (test mode)</strong></p>
        </div>
      </div>
      <aside style={{width:320}} className="card aside-cart">
        <h4>Order Summary</h4>
        <p className="small">Items: {cart.length}</p>
        <p className="price">Total: ₹{total}</p>
        <button className="button" onClick={doPlace} disabled={cart.length===0}>Place order (test)</button>
      </aside>
    </div>
  );
}
