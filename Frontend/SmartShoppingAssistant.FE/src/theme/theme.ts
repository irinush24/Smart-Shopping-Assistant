// export const themeOptions: ThemeOptions = {
//   palette: {
//     mode: 'light',
//     primary: {
//       main: '#c2185b',
//     },
//     secondary: {
//       main: '#f50057',
//     },
//     background: {
//       default: '#fce4ec',
//     },
//     error: {
//       main: '#b71c1c',
//     },
//     text: {
//       disabled: '#44397b',
//       secondary: 'rgba(61,44,44,0.6)',
//     },
//   },
//   typography: {
//     fontFamily: 'Sniglet',
//     h1: {
//       fontFamily: 'Lavishly Yours',
//     },
//     h2: {
//       fontFamily: 'Lavishly Yours',
//     },
//     h3: {
//       fontFamily: 'Lavishly Yours',
//     },
//     h4: {
//       fontFamily: 'Lavishly Yours',
//     },
//     h5: {
//       fontFamily: 'Lavishly Yours',
//     },
//     h6: {
//       fontFamily: 'Lavishly Yours',
//     },
//     subtitle1: {
//       fontFamily: 'Corinthia',
//     },
//     subtitle2: {
//       fontFamily: 'Corinthia',
//     },
//   },
// };

// const theme = createTheme(themeOptions);

// export default theme;

// import { createTheme } from '@mui/material/styles';

// export const appTheme = createTheme({
//   palette: {
//     primary: {
//       main: '#c2185b',
//       light: '#f48fb1',
//     },
//     background: {
//       default: '#fce4ec', // Your light baby pink background
//       paper: '#ffffff',
//     },
//     text: {
//       primary: '#4a2333', // A very dark, warm brown/pink instead of harsh pure black
//       secondary: '#885b6f',
//     }
//   },
//   typography: {
//     // A soft, rounded, modern font
//     fontFamily: '"Quicksand", "Roboto", "Helvetica", "Arial", sans-serif',
//     h4: { fontWeight: 600 },
//     h6: { fontWeight: 600 },
//     button: {
//       fontWeight: 600,
//       textTransform: 'none', // Removes the default ALL CAPS from buttons to make them friendlier
//     },
//   },
//   shape: {
//     borderRadius: 16, // Makes all default MUI components beautifully rounded
//   },
//   components: {
//     // Soften the Cards
//     MuiCard: {
//       styleOverrides: {
//         root: {
//           borderRadius: 24, // Extra squishy corners for product cards
//           border: 'none',
//           // A custom, soft pink-tinted shadow instead of a harsh gray one
//           boxShadow: '0 8px 24px rgba(194, 24, 91, 0.08)',
//           transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
//           '&:hover': {
//             transform: 'translateY(-4px)', // Makes cards "lift" when you hover over them
//             boxShadow: '0 12px 32px rgba(194, 24, 91, 0.12)',
//           },
//         },
//       },
//     },
//     // Soften the Buttons
//     MuiButton: {
//       styleOverrides: {
//         root: {
//           borderRadius: 50, // Pill-shaped buttons
//           padding: '8px 24px',
//           boxShadow: 'none', // Flat, modern look
//           '&:hover': {
//             boxShadow: '0 4px 12px rgba(194, 24, 91, 0.2)',
//           },
//         },
//       },
//     },
//     // Soften the Dialogs (Like your AI Cart Analysis)
//     MuiDialog: {
//       styleOverrides: {
//         paper: {
//           borderRadius: 24,
//           padding: '8px',
//           boxShadow: '0 24px 48px rgba(194, 24, 91, 0.15)',
//         },
//       },
//     },
//   },
// });

import { createTheme } from '@mui/material/styles';

export const appTheme = createTheme({
  palette: {
    primary: {
      main: '#c2185b',
      light: '#ffb3c6', // Pastel Pink
      dark: '#c9184a', // Deep Berry
      contrastText: '#ffffff',
    },
    background: {
      default: '#fff0f3', // Creamy pale strawberry milk
      paper: '#ffffff', // Frosting white for cards
    },
    text: {
      primary: '#590d22', // Deep cherry/chocolate brown
      secondary: '#a4133c', // Muted berry
    }
  },
  typography: {
    fontFamily: '"Nunito", "Quicksand", sans-serif',
    h1: { fontFamily: '"Pacifico", cursive', color: '#c2185b' },
    h2: { fontFamily: '"Pacifico", cursive', color: '#c2185b' },
    h3: { fontFamily: '"Pacifico", cursive', color: '#c2185b' },
    h4: { fontFamily: '"Pacifico", cursive', color: '#c2185b' },
    h5: { fontFamily: '"Pacifico", cursive', color: '#c2185b' },
    h6: { fontFamily: '"Pacifico", cursive', color: '#c2185b' },
    button: {
      fontWeight: 700,
      textTransform: 'none',
      letterSpacing: '0.5px',
    },
  },
  shape: {
    borderRadius: 30,
  },
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        html: {
          fontSize: "80%",
        },
        body: {
          backgroundColor: '#fff0f3', // The Vanilla Cream base
          backgroundImage: `
            linear-gradient(rgba(255, 179, 198, 0.15) 50%, transparent 50%),
            linear-gradient(90deg, rgba(255, 179, 198, 0.15) 50%, transparent 50%)
          `,
          backgroundSize: '60px 60px', // Change this to make the squares bigger or smaller!
          
          // The pattern will stay still while scrolling
          backgroundAttachment: 'fixed', 
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 24,
          border: '2px dashed #ffb3c6',
          boxShadow: '0 10px 20px rgba(255, 77, 109, 0.1)',
          transition: 'transform 0.3s ease, box-shadow 0.3s ease',
          '&:hover': {
            transform: 'translateY(-6px) scale(1.02)',
            boxShadow: '0 15px 30px rgba(255, 77, 109, 0.2)',
          },
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        contained: {
          borderRadius: 50,
          padding: '10px 28px',
          boxShadow: '0 6px 15px rgba(194, 37, 66, 0.3)',
          '&:hover': {
            boxShadow: '0 8px 20px rgba(194, 37, 66, 0.4)',
            backgroundColor: '#ff3b5c',
          },
        },
        text: {
          '&:hover': {
            backgroundColor: 'rgba(194, 37, 66, 0.08)',
          }
        }
      },
    },
    MuiDialog: {
      styleOverrides: {
        paper: {
          borderRadius: 32,
          padding: '12px',
          border: '4px solid #fff0f3',
          boxShadow: '0 25px 50px rgba(89, 13, 34, 0.15)',
        },
      },
    },
  },
});

export default appTheme;