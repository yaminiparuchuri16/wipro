import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { register } from '../redux/slices/authSlice';
import { useNavigate } from 'react-router-dom';

export default function Register(){
  const [email,setEmail]=useState('');
  const [password,setPassword]=useState('');
  const dispatch = useDispatch();
  const nav = useNavigate();

  const doRegister = async ()=>{
    try{
      await dispatch(register({ email, password })).unwrap();
      alert('Registered - please login');
      nav('/login');
    }catch(e){ alert('Register failed'); }
  };

  return (
    <div style={{maxWidth:480, margin:'0 auto'}}>
      <h3>Register</h3>
      <div className="card">
        <div className="form-row"><input className="input" placeholder="Email" value={email} onChange={e=>setEmail(e.target.value)} /></div>
        <div className="form-row"><input type="password" className="input" placeholder="Password" value={password} onChange={e=>setPassword(e.target.value)} /></div>
        <button className="button" onClick={doRegister}>Register</button>
      </div>
    </div>
  );
}
