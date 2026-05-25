import { createTheme, type ThemeOptions } from '@mui/material/styles';

export const themeOptions: ThemeOptions = {
  palette: {
    mode: 'light',
    primary: {
      main: '#c2185b',
    },
    secondary: {
      main: '#f50057',
    },
    background: {
      default: '#fce4ec',
    },
    error: {
      main: '#b71c1c',
    },
    text: {
      disabled: '#44397b',
      secondary: 'rgba(61,44,44,0.6)',
    },
  },
  typography: {
    fontFamily: 'Sniglet',
    h1: {
      fontFamily: 'Lavishly Yours',
    },
    h2: {
      fontFamily: 'Lavishly Yours',
    },
    h3: {
      fontFamily: 'Lavishly Yours',
    },
    h4: {
      fontFamily: 'Lavishly Yours',
    },
    h5: {
      fontFamily: 'Lavishly Yours',
    },
    h6: {
      fontFamily: 'Lavishly Yours',
    },
    subtitle1: {
      fontFamily: 'Corinthia',
    },
    subtitle2: {
      fontFamily: 'Corinthia',
    },
  },
};

const theme = createTheme(themeOptions);

export default theme;