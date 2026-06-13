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
import AdminRoute from './components/common/AdminRoute'

function App() {
  return (
    <CartProvider>
      <Box className="app">
        <NavBar/>
        <Routes>
            {/*Public routes that both roles can access*/}
            <Route path = "/" element = {<Home />} />
            <Route path = "/shop" element = {<Shop />} />
            <Route path = "*" element = {<NotFound />} />

            {/*Protected Admin routes that are wrapped inside the guard*/}
            <Route element = {<AdminRoute/>}>
              <Route path = "/categories" element = {<Categories />} />
              <Route path = "/products" element = {<Products />} />      
              <Route path = "/promotions" element = {<Promotions />} />        
            </Route>
        </Routes>
        <CartDrawer />
      </Box>
    </CartProvider>
  );
}

export default App;
