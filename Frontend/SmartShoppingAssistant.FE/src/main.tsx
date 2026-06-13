import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { BrowserRouter } from 'react-router-dom'
import {ThemeProvider} from "@mui/material";
import CssBaseline from "@mui/material/CssBaseline";
import { appTheme } from "./theme/theme.ts"

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
        <ThemeProvider theme={appTheme}>
            <CssBaseline />
            {/* Applies the background color globally */}
            <App />
        </ThemeProvider>
    </BrowserRouter>
  </StrictMode>,
);
