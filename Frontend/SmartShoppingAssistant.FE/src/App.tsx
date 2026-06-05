import './App.css'
import NavBar from './components/navbar'
import { Box } from "@mui/material"
import { Route, Routes } from "react-router-dom"
import Home from "./components/Home"
import Shop from "./components/Shop"
import CartDrawer from './components/CartDrawer'
import Categories from './components/Categories'
import Products from './components/Products'
import Promotions from './components/Promotions'
import NotFound from './components/NotFound'
import CartProvider from './context/CartContext/CartProvider'

function App() {
  return (
    <CartProvider>
      <Box className="app">
        <NavBar/>
        <Routes>
            <Route path = "/" element = {<Home />} />
            <Route path = "/shop" element = {<Shop />} />
            <Route path = "/categories" element = {<Categories />} />
            <Route path = "/products" element = {<Products />} />      
            <Route path = "/promotions" element = {<Promotions />} />        
            <Route path = "*" element = {<NotFound />} />
        </Routes>
        <CartDrawer />
      </Box>
    </CartProvider>
  );
}

export default App;
