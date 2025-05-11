import React from 'react';
import { Container, Typography, Box } from '@mui/material';
import PrescriptionList from '../components/PrescriptionList';

const PrescriptionsPage: React.FC = () => {
    return (
        <Container>
            <Box my={4}>
                <Typography variant="h4" component="h1" gutterBottom>
                    Physiotherapy Prescriptions
                </Typography>
                <Typography variant="body1" color="text.secondary" paragraph>
                    Create and manage exercise prescriptions for your patients
                </Typography>
                <PrescriptionList />
            </Box>
        </Container>
    );
};

export default PrescriptionsPage;