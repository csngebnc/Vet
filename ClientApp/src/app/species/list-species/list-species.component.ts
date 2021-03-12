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
  constructor(private speciesService: SpeciesService, private modalService: NgbModal) {
    this.refreshSpecies();
  }

  ngOnInit(): void {

  }

  open() {
    const modalRef = this.modalService.open(AddSpeciesComponent);
    modalRef.result.then(() => this.refreshSpecies(), () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditSpeciesComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then(() => this.refreshSpecies(), () => { })
  }

  refreshSpecies() {
    this.speciesService.getAnimalSpecies().subscribe((species: SpeciesDto[]) => {
      this.species = species;
      this.dataSource = new MatTableDataSource<SpeciesDto>(this.species);
    })
  }

  changeStateOfSpecies(id) {
    this.speciesService.changeState(id).subscribe(() => {
      this.refreshSpecies();
    })
  }

  deleteSpecies(id) {
    this.speciesService.deleteSpecies(id).subscribe(() => { this.refreshSpecies() });
  }

}
