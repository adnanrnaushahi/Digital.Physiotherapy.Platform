import axios from 'axios';
import type { Patient } from '../models/Patient';
import type { Exercise } from '../models/Exercise';
import { type Prescription, type PrescriptionCreate } from '../models/Prescription';
import { type SessionResult, type SessionResultCreate } from '../models/SessionResult';

// Create axios instance with base URL
const api = axios.create({
    baseURL: 'http://localhost:5218/api',  // Update to match your .NET API port
    headers: {
        'Content-Type': 'application/json'
    }
});

// Prescriptions API
export const getPrescriptions = async (): Promise<Prescription[]> => {
    const response = await api.get<Prescription[]>('/prescriptions');
    return response.data;
};

export const getPrescriptionById = async (id: number): Promise<Prescription> => {
    const response = await api.get<Prescription>(`/prescriptions/${id}`);
    return response.data;
};

export const createPrescription = async (prescription: PrescriptionCreate): Promise<Prescription> => {
    const response = await api.post<Prescription>('/prescriptions', prescription);
    return response.data;
};

// Patients API
export const getPatients = async (): Promise<Patient[]> => {
    const response = await api.get<Patient[]>('/prescriptions/patients');
    return response.data;
};

// Exercises API
export const getExercises = async (): Promise<Exercise[]> => {
    const response = await api.get<Exercise[]>('/prescriptions/exercises');
    return response.data;
};

// Session Results API
export const getSessionResults = async (): Promise<SessionResult[]> => {
    const response = await api.get<SessionResult[]>('/results');
    return response.data;
};

export const getSessionResultById = async (id: number): Promise<SessionResult> => {
    const response = await api.get<SessionResult>(`/results/${id}`);
    return response.data;
};

export const getSessionResultsByPrescription = async (prescriptionId: number): Promise<SessionResult[]> => {
    const response = await api.get<SessionResult[]>(`/results/prescription/${prescriptionId}`);
    return response.data;
};

export const createSessionResult = async (result: SessionResultCreate): Promise<SessionResult> => {
    const response = await api.post<SessionResult>('/results', result);
    return response.data;
};