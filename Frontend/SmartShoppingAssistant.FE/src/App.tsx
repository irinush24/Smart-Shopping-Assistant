import './App.css'
import NavBar from './components/navbar'
import { Box } from "@mui/material"
import {Route, Routes} from "react-router-dom"
import Home from "./components/Home"
import Categories from './components/Categories'
import Products from './components/Products'

function App() {
  return (
      <Box className="hero">
        <NavBar/>
          <Routes>
              <Route path = "/" element = {<Home />} />
              <Route path = "/categories" element = {<Categories />} />
              <Route path = "/products" element = {<Products />} />              
          </Routes>
      </Box>
      
  );
}

export default App
