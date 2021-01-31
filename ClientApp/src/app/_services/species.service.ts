import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SpeciesService {
  baseUrl: string = 'https://localhost:44345/api/'

  constructor(private http: HttpClient) { }

  addSpecies(model: any){
    return this.http.post(this.baseUrl + 'species', model);
  }

  updateSpecies(model: any){
    return this.http.put(this.baseUrl + 'species', model);
  }

  changeState(id){
    return this.http.put(this.baseUrl + 'species/state/' + id, null);
  }

  deleteSpecies(id){
    return this.http.delete(this.baseUrl + 'species/' + id);
  }

  getAnimalSpeciesById(id){
    return this.http.get(this.baseUrl + 'species/' + id);
  }

  getAnimalSpecies(){
    return this.http.get(this.baseUrl + 'species')
  }
}
