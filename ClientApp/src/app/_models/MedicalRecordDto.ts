import { TherapiaRecordDto } from "./therapiarecorddto";

export interface MedicalRecordDto {
    id: number;
    date: string;
    doctorId: string;
    doctorName: string;
    ownerEmail: string;
    ownerId: string;
    ownerName: string;
    animalId: number;
    animalName: string;
    anamnesis: string;
    symptoma: string;
    details: string;
    therapiaRecords: TherapiaRecordDto[];
    photos: MedicalRecordPhotoDto[];
}

export interface MedicalRecordPhotoDto {
    id: number;
    medicalRecordId: number;
    path: string;
}
