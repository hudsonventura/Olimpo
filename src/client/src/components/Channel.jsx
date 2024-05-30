import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';

function Channel({title, value, unit, type}) {
    return (
        <Toast style={{marginLeft: '9px', width:"230px"}}>
            <a style={{fontSize: '12px'}}>{(title.length > 30) ? title.substr(0, 35) + ' ...' : title}</a>
            <div style={{display: 'flex', justifyContent: 'flex-end', width: '100%'}}>
                <Badge style={{margin: '10px'}} bg={type}>{value} {unit}</Badge>
            </div>
        </Toast>
    );
}

export default Channel;