import { AppBar, Toolbar, Button, Box, ToggleButtonGroup, ToggleButton, IconButton, Badge } from "@mui/material"
import { NavLink, Link, useNavigate } from "react-router-dom";
import logo from "../../assets/logo.png"
import React, { useState } from "react";
import {useCart} from "../../context/CartContext/cart-context"
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart'

function NavBar()
{
    const [mode, setMode] = useState<"user" | "admin">("user")
    const { cart, openCart } = useCart();

    const navigate = useNavigate();
    const handleModeChange = (_event : React.MouseEvent<HTMLElement>, value: "user" | "admin") => {
       setMode(value)
       navigate('/')
    }

    return (<AppBar position="static" elevation={0}>
            <Toolbar>
                <Link to = "/">
                    <Box
                        component = "img"
                        src = {logo}
                        alt = "Handmade Hugs by Iri Logo"
                        sx = {{ height : 62, mr: 2}}
                    />
                </Link>
                <Box sx = {{display: 'flex', flexGrow: 1}}>
                    <Button component = {NavLink} to="/" variant = "text" color="inherit">Home</Button>
                    {mode === "admin" ? (
                        // Admin-specific links in a React Fragment
                        <>  
                            <Button component = {NavLink} to="/categories" variant = "text" color="inherit">Categories</Button>
                            <Button component = {NavLink} to="/products" variant = "text" color="inherit">Products</Button>
                            <Button component = {NavLink} to="/promotions" variant = "text" color="inherit">Promotions</Button>
                        </>
                    ) : 
                    (
                        // User-specific links in a React Fragment
                        <Button component = {NavLink} to="/shop" variant = "text" color="inherit">Shop</Button>
                    )}
                </Box>

                <ToggleButtonGroup value = {mode} exclusive size = "small" sx = {{mr : 2}} onChange={handleModeChange}
                >
                    <ToggleButton value = "user">User</ToggleButton>
                    <ToggleButton value = "admin">Admin</ToggleButton>
                </ToggleButtonGroup>

                {mode === 'user' && (
          <IconButton color="inherit" onClick={openCart}>
            <Badge badgeContent={cart?.itemCount ?? 0} color="primary">
              <ShoppingCartIcon />
            </Badge>
          </IconButton>
        )}
        </Toolbar>
    </AppBar>
    );
}

export default NavBar;