import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import UsersGrid from './UsersGrid';

function App() {
  const [file, setFile] = useState(null);

  const handleFileChange = (e: any) => {
    setFile(e.target.files[0]);
  };

  const handleSubmit = async (e: any) => {
    e.preventDefault();
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    try {
      const res = await fetch('https://localhost:7216/Upload', {
        method: 'POST',
        body: formData,
      });

      const result = await res.json();

      alert(result.message);
    } catch (err) {
      alert('Error uploading file');
    }
  };

  return (
    <>
      <div style={{ display: 'flex', justifyContent: 'center', gap: '1rem', paddingTop: '1rem' }}>
        <a href="#" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" style={{ width: '100px' }} />
        </a>
        <h1 style={{ textAlign: 'center' }}>CertifyMe</h1>
        <a href="#" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" style={{ width: '100px' }} />
        </a>
      </div>

      <div>
        <div>
          <h2>Import</h2>
          <form onSubmit={handleSubmit}>
            <input title="file" type="file" onChange={handleFileChange} />
            <button type="submit">Upload</button>
          </form>
        </div>

        <div>
          <h2>Course Members</h2>
          <UsersGrid />
        </div>
      </div>
    </>
  );
}

export default App
