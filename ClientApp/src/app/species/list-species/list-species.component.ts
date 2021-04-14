import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SpeciesDto } from 'src/app/_models/speciesdto';
import { SpeciesService } from 'src/app/_services/species.service';
import { AddSpeciesComponent } from '../add-species/add-species.component';
import { EditSpeciesComponent } from '../edit-species/edit-species.component';

@Component({
  selector: 'app-list-species',
  templateUrl: './list-species.component.html',
  styleUrls: ['./list-species.component.css']
})
export class ListSpeciesComponent implements OnInit {

  species: SpeciesDto[] = [];

  dataSource;
  displayedColumns: string[] = ['id', 'name', 'status', 'button'];
  constructor(private speciesService: SpeciesService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.speciesService.getAnimalSpecies().subscribe((species: SpeciesDto[]) => {
      this.species = species;
      this.dataSource = new MatTableDataSource<SpeciesDto>(this.species);
    })
  }

  open() {
    const modalRef = this.modalService.open(AddSpeciesComponent);
    modalRef.result.then((spec: SpeciesDto) => {
      this.species.push(spec);
      this.dataSource = new MatTableDataSource<SpeciesDto>(this.species);
    }, () => { })
  }

  openEdit(id: number) {
    const modalRef = this.modalService.open(EditSpeciesComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then((spec: SpeciesDto) => {
      this.species[this.species.map(s => s.id).indexOf(id)] = spec;
      this.dataSource = new MatTableDataSource<SpeciesDto>(this.species);
    }, () => { })
  }

  changeStateOfSpecies(id: number) {
    this.speciesService.changeState(id).subscribe(() => {
      this.species[this.species.map(s => s.id).indexOf(id)].isInactive = !this.species[this.species.map(s => s.id).indexOf(id)].isInactive;
      this.dataSource = new MatTableDataSource<SpeciesDto>(this.species);
    })
  }

  deleteSpecies(id: number) {
    this.speciesService.deleteSpecies(id).subscribe(() => {
      this.species = this.species.filter(s => s.id !== id);
      this.dataSource = new MatTableDataSource<SpeciesDto>(this.species);
    });
  }

}
