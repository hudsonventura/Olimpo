import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Toasty from '../components/Toasty';


function ToastyGroup({success, warning, error}) {
    return (
        <Container style={{marginTop: '15px'}}>
            <Row>
                <Col>
                    <Toasty type="success" value={success} text={"Ok"} title="Number of channels online"></Toasty>
                </Col>
                <Col>
                    <Toasty type="warning" value="Fake" text={"Warnning"} title="Number of channels with some alert"></Toasty>
                </Col>
                <Col>
                    <Toasty type="danger"  value={error} text={"Error"} title="Number of channels offline / error"></Toasty>
                </Col>
            </Row>
        </Container>
    );
}

export default ToastyGroup;