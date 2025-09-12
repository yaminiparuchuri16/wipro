import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { login } from '../redux/slices/authSlice';
import { useNavigate } from 'react-router-dom';

export default function Login(){
  const [email,setEmail]=useState('');
  const [password,setPassword]=useState('');
  const dispatch = useDispatch();
  const nav = useNavigate();

  const doLogin = async ()=>{
    try{
      await dispatch(login({ email, password })).unwrap();
      nav('/');
    }catch(e){ alert('Login failed'); }
  };

  return (
    <div style={{maxWidth:480, margin:'0 auto'}}>
      <h3>Login</h3>
      <div className="card">
        <div className="form-row"><input className="input" placeholder="Email" value={email} onChange={e=>setEmail(e.target.value)} /></div>
        <div className="form-row"><input type="password" className="input" placeholder="Password" value={password} onChange={e=>setPassword(e.target.value)} /></div>
        <button className="button" onClick={doLogin}>Login</button>
      </div>
    </div>
  );
}
