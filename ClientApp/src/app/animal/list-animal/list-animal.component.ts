import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AnimalDto } from 'src/app/_models/animaldto';
import { AnimalService } from 'src/app/_services/animal.service';
import { AddAnimalComponent } from '../add-animal/add-animal.component';

@Component({
  selector: 'app-list-animal',
  templateUrl: './list-animal.component.html',
  styleUrls: ['./list-animal.component.css']
})
export class ListAnimalComponent implements OnInit {

  animals: AnimalDto[] = [];
  page = 1;
  pageSize = 8;

  constructor(private animalService: AnimalService, private modalService: NgbModal) { }

  open() {
    const modalRef = this.modalService.open(AddAnimalComponent);
    modalRef.result.then((animal) => {
      this.animals.push(animal)
    }, () => { })
  }

  ngOnInit(): void {
    this.animalService.getUserAnimals().subscribe((animals: AnimalDto[]) => {
      this.animals = animals;
    })
  }

  deleteAnimal(id: number) {
    if (confirm('Biztosan szeretnéd törölni?')) {
      this.animalService.deleteAnimal(id).subscribe(() => {
        let deleteIdx = this.animals.map((a) => { return a.id }).indexOf(id);
        this.animals.splice(deleteIdx, 1);
      });
    }
  }

  archiveAnimal(id) {
    if (confirm('Biztosan szeretnéd archiválni?')) {
      this.animalService.changeStateOfAnimal(id).subscribe(() => {
        let deleteIdx = this.animals.map((a) => { return a.id }).indexOf(id);
        this.animals.splice(deleteIdx, 1);
      });
    }
  }

}
