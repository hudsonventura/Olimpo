import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';

function Toasty({type, value, text}) {
  return (
    <Toast style={{margin: '10px'}}>
      <Toast.Body><h1><Badge bg={type}>{value}</Badge> {text}</h1></Toast.Body>
    </Toast>
  );
}

export default Toasty;