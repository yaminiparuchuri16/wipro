import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { logout } from '../redux/slices/authSlice';

export default function Navbar(){
  const cart = useSelector(s => s.cart.items);
  const auth = useSelector(s => s.auth);
  const dispatch = useDispatch();
  const nav = useNavigate();

  const doLogout = ()=>{ dispatch(logout()); nav('/'); };

  return (
    <nav className="navbar">
      <div className="brand" style={{
        background: 'linear-gradient(90deg, #1e3a8a 0%, #b7e7a7 60%, #a75a7c 100%)',
        color: '#fff',
        borderRadius: '12px',
        padding: '8px 24px',
        fontWeight: 'bold',
        fontSize: '2rem',
        boxShadow: '0 2px 12px #b7e7a7',
        display: 'inline-block',
        marginRight: '16px',
      }}>
        <Link to="/" style={{textDecoration:'none', color:'inherit'}}>E-Shop</Link>
      </div>
      <div className="nav-links">
  <Link to="/" style={{display:'inline-flex',alignItems:'center',gap:4,fontWeight:'bold'}}><span role="img" aria-label="home">🏠</span> Home</Link>
  <Link to="/products" style={{display:'inline-flex',alignItems:'center',gap:4,fontWeight:'bold'}}><span role="img" aria-label="products">📦</span> Products</Link>
        {/* <Link to="/admin">Admin</Link> */}
  <Link to="/admin-login" style={{display:'inline-flex',alignItems:'center',gap:4,fontWeight:'bold'}}><span role="img" aria-label="admin">🛡️</span> Admin Login</Link>
        <Link to="/cart" style={{display:'inline-flex',alignItems:'center',gap:4}}><span role="img" aria-label="cart">🛒</span> Cart ({cart.length})</Link>
        {auth.user ? (
          <>
            <Link to="/orders">Orders</Link>
            <a onClick={doLogout} style={{cursor:'pointer'}}>Logout</a>
          </>
        ) : (
          <>
            <Link to="/login" style={{display:'inline-flex',alignItems:'center',gap:4,fontWeight:'bold'}}><span role="img" aria-label="login">🔑</span> Login</Link>
            <Link to="/register" style={{display:'inline-flex',alignItems:'center',gap:4,fontWeight:'bold'}}><span role="img" aria-label="register">📝</span> Register</Link>
          </>
        )}
      </div>
    </nav>
  );
}
