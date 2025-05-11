import React from 'react';
import {
    Container,
    Typography,
    Box,
    Card,
    CardContent,
    Grid,
    Button
} from '@mui/material';
import { Link } from 'react-router-dom';
import FitnessCenterIcon from '@mui/icons-material/FitnessCenter';
import AssessmentIcon from '@mui/icons-material/Assessment';

const Homepage: React.FC = () => {
    return (
        <Container maxWidth="lg">
            <Box
                sx={{
                    mt: 8,
                    mb: 4,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center'
                }}
            >
                <Typography variant="h2" component="h1" align="center" gutterBottom>
                    Digital Physiotherapy Platform
                </Typography>
                <Typography variant="h5" align="center" color="text.secondary" paragraph>
                    Manage exercise and prescriptions with our digital platform.
                </Typography>
            </Box>

            <Grid container spacing={4} sx={{ mb: 8 }}>
                <Grid item xs={12} md={6}>
                    <Card
                        sx={{
                            height: '100%',
                            display: 'flex',
                            flexDirection: 'column',
                            transition: 'transform 0.2s',
                            '&:hover': {
                                transform: 'scale(1.02)',
                                boxShadow: 3
                            }
                        }}
                    >
                        <CardContent sx={{ flexGrow: 1, textAlign: 'center', p: 4 }}>
                            <FitnessCenterIcon sx={{ fontSize: 60, mb: 2, color: 'primary.main' }} />
                            <Typography gutterBottom variant="h4" component="h2">
                                Exercise Prescriptions
                            </Typography>
                            <Typography variant="body1" paragraph>
                                Create and view exercise plans for patients with detailed instructions.
                            </Typography>
                            <Button
                                component={Link}
                                to="/prescriptions"
                                variant="contained"
                                size="large"
                                sx={{ mt: 2 }}
                            >
                                Manage Prescriptions
                            </Button>
                        </CardContent>
                    </Card>
                </Grid>

                <Grid item xs={12} md={6}>
                    <Card
                        sx={{
                            height: '100%',
                            display: 'flex',
                            flexDirection: 'column',
                            transition: 'transform 0.2s',
                            '&:hover': {
                                transform: 'scale(1.02)',
                                boxShadow: 3
                            }
                        }}
                    >
                        <CardContent sx={{ flexGrow: 1, textAlign: 'center', p: 4 }}>
                            <AssessmentIcon sx={{ fontSize: 60, mb: 2, color: 'primary.main' }} />
                            <Typography gutterBottom variant="h4" component="h2">
                                Session Results
                            </Typography>
                            <Typography variant="body1" paragraph>
                                Review patient progress with detailed session results.
                            </Typography>
                            <Button
                                component={Link}
                                to="/results"
                                variant="contained"
                                size="large"
                                sx={{ mt: 2 }}
                            >
                                View Results
                            </Button>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
        </Container>
    );
};

export default Homepage;