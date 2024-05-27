import logo from './../logo.svg';


import { Tree } from 'primereact/tree';
        



import 'primereact/resources/themes/lara-light-cyan/theme.css';
import { Button } from 'primereact/button';

let nodes = [
    {
        "label": "1 > Documents",
        "data": {id:1},
        "expandedIcon": "fa-folder-open",
        "collapsedIcon": "fa-folder",
        "children": [{
            "label": "111 > Work",
            "data": {id: 111},
            "expandedIcon": "fa-folder-open",
            "collapsedIcon": "fa-folder",
            "children": [
              { "label": "11 > Expenses.doc", "icon": "fa-file-word-o", "data":{id:11} }, 
            { "label": "12 > Resume.doc", "icon": "fa-file-word-o",  "data":{id:12} }
            ]
        },
        {
            "label": "112 > Home",
            "data": {id: 112},
            "expandedIcon": "fa-folder-open",
            "collapsedIcon": "fa-folder",
            "children": [{ "label": "21> Invoices.txt", "icon": "fa-file-word-o", "data":{id:21}}]
        }]
    },
    {
        "label": "2> Pictures",
        "data": {id: 2},
        "expandedIcon": "fa-folder-open",
        "collapsedIcon": "fa-folder",
        "children": [
            { "label": "31 >barcelona.jpg", "icon": "fa-file-image-o", "data":{id:31} },
            { "label": "32 > logo.jpg", "icon": "fa-file-image-o", "data":{id:32} },
            { "label": "33> primeui.png", "icon": "fa-file-image-o", "data":{id:33} }]
    }
   
]

function Main() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>

        <Button label="PrimeReact" />
        <Tree value={nodes} className="w-full md:w-30rem" />

        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default Main;
