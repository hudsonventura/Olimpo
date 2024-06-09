import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import Badge from 'react-bootstrap/Badge';
import Col from 'react-bootstrap/Col';

import Form from 'react-bootstrap/Form';


import CalcChannels, { countChannelsInAllStacks_Error } from '../components/CalcChannels';



function Navigation({data, readOnlyMode, setReadOnlyMode}) {

    if(readOnlyMode == undefined){
        readOnlyMode = true;
    }

    return (
        <Navbar expand="lg" className="bg-body-tertiary" sticky="top" bg="dark" data-bs-theme="dark" >

            <Navbar.Brand style={{ marginLeft: "20px" }} href="#home"><img alt="" src="/logo.png" height="30" className="d-inline-block align-top" /> Olimpo </Navbar.Brand>
            <Col>
                <Badge bg={'danger'}>{countChannelsInAllStacks_Error(data)}</Badge> <a style={{color: "white"}}>Channel(s) with error(s)</a>
            </Col>
                
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="me-auto">
                    <Nav.Link href="/Main">Home</Nav.Link>
                    <Nav.Link href="/ListErrors">List Errors</Nav.Link>
                </Nav>
                <Nav className="me-auto"></Nav>
                <Nav className="me-auto"></Nav>
                <Nav className="me-auto">
                <NavDropdown title="Username" id="basic-nav-dropdown" className="">
                        <NavDropdown.Item href="#action/3.1">
                            <Form.Check
                                type="switch"
                                id="custom-switch"
                                label="Readonly Mode"
                                checked={readOnlyMode}
                                onChange={() => setReadOnlyMode(!readOnlyMode)}
                            />
                        </NavDropdown.Item>
                        <NavDropdown.Item href="#action/3.2">
                            Another action
                        </NavDropdown.Item>
                        <NavDropdown.Item href="#action/3.3">Something</NavDropdown.Item>
                        <NavDropdown.Divider />
                        <NavDropdown.Item href="#action/3.4">
                            Logout
                        </NavDropdown.Item>
                    </NavDropdown>
                </Nav>
            </Navbar.Collapse>

        </Navbar>
    );
}

export default Navigation;
