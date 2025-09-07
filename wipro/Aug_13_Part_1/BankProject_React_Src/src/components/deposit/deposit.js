import axios from 'axios';
import React, {Component, useState} from 'react';

const Deposit = () => {
      const [result,SetResult] = useState('');
       const [data, setData] = useState({
          accountNo : 0,
          depositAmount : 0,
      })
  
     const handleChange = event => {
        setData({
            ...data,[event.target.name] : event.target.value  
        })
    }

    const depositAccount = () => {
     axios.post("https://localhost:7006/deposit/"+data.accountNo + "/"+data.depositAmount).then(resp => {
        //   alert(resp.data);
        SetResult(resp.data);
          console.log(resp.data);
        })
    }

      return (
    <div>
            <label>Account No : </label>
            <input type="number" name="accountNo" 
                value={data.accountNo} onChange={handleChange} /> <br/><br/> 
            <label>Deposit Amount : </label>
            <input type="number" name="depositAmount" 
                value={data.depositAmount} onChange={handleChange} /> <br/><br/> 
 
            <input type="button" value="Deposit Account" onClick={depositAccount} /> 
            <hr/>
            <b>{result}</b>
    </div>
  )

}

export default Deposit;