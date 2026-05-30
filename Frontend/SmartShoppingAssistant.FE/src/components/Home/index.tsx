import {Container, Typography} from "@mui/material"

function Home()
{
    return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h3" gutterBottom>
        Smart Shopping Assistant
      </Typography>
      <Typography>Welcome! Your shopping assistant starts here.</Typography>
    </Container>
  );
}

export default Home;