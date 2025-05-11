import React from 'react';
import { Link } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import FitnessCenterIcon from '@mui/icons-material/FitnessCenter';

const Navbar: React.FC = () => {
    return (
        <AppBar position="static">
            <Toolbar>
                <FitnessCenterIcon sx={{ mr: 2 }} />
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Digital Physio
                </Typography>
                <Box>
                    <Button
                        color="inherit"
                        component={Link}
                        to="/">
                        Home
                    </Button>
                    <Button
                        color="inherit"
                        component={Link}
                        to="/prescriptions">
                        Prescriptions
                    </Button>
                    <Button
                        color="inherit"
                        component={Link}
                        to="/results">
                        Results
                    </Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Navbar;