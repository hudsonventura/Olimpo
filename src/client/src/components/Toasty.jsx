import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';

function Toasty({type, value, text, title}) {
    return (
        <Toast style={{margin: '10px'}}>
        <Toast.Header closeButton={false}>
            <img src="holder.js/20x20?text=%20" className="rounded me-2" alt="" />
            <strong className="me-auto">{title}</strong>
            {/* <small>11 mins ago</small> */}
        </Toast.Header>
        <Toast.Body><h1><Badge bg={type}>{value}</Badge> {text}</h1></Toast.Body>
        </Toast>
    );
}

export default Toasty;