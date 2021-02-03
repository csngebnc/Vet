export interface TreatmentTimeDto{
    id: number
    startHour: number
    startMin: number
    endHour: number
    endMin: number
    duration: number
    dayOfWeek: number
    treatmentId: number
    isInactive: boolean
}
