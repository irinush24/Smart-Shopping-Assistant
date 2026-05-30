import {Container, Typography} from "@mui/material"

function Home()
{
    return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h1" gutterBottom>
        Handmade Hugs by Iri
      </Typography>
      <Typography>Welcome!</Typography>
    </Container>
  );
}

export default Home;