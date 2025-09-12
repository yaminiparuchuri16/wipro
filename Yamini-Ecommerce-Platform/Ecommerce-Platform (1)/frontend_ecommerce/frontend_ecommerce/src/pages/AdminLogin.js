import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function AdminLogin() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const nav = useNavigate();

  const handleLogin = (e) => {
    e.preventDefault();
    if (email === 'yamini@gmail.com' && password === '12345') {
      localStorage.setItem('adminLoggedIn', 'true');
      nav('/admin-dashboard');
    } else {
      setError('Invalid credentials');
    }
  };

  return (
    <div style={{maxWidth:480, margin:'0 auto'}}>
      <h3>Admin Login</h3>
      <div className="card">
        <form onSubmit={handleLogin}>
          <div className="form-row"><input className="input" type="email" placeholder="Email" value={email} onChange={e=>setEmail(e.target.value)} required /></div>
          <div className="form-row"><input className="input" type="password" placeholder="Password" value={password} onChange={e=>setPassword(e.target.value)} required /></div>
          <button className="button" type="submit">Login</button>
          {error && <div style={{color:'red',marginTop:12}}>{error}</div>}
        </form>
      </div>
    </div>
  );
}
